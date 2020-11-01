using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using MySql.Common;
using System.Linq;

namespace Messenger.Common.JWT
{
    public class JwtHelper
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private string m_issuer = "AuthServer";
        private SymmetricSecurityKey m_securityKey;

        public const string AccessTokenName = "access_token";
        public const string RefreshTokenName = "refresh_token";

        public JwtHelper(string secretKeyPath)
        {
            if (!File.Exists(secretKeyPath))
            {
                throw new ApplicationException("Secret JWT key does not exists");
            }

            byte[] secret = File.ReadAllBytes(secretKeyPath);
            m_securityKey = new SymmetricSecurityKey(secret);
        }

        public List<string> GetRoles(string accessToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.ReadJwtToken(accessToken);
            foreach (Claim claim in token.Claims)
            {
                if (claim.Type == ClaimTypes.Role || claim.Type == "role")
                {
                    string roles = claim.Value;
                    if (string.IsNullOrEmpty(roles))
                    {
                        return null;
                    }

                    return roles.Split(',').ToList();
                }
            }

            return null;
        }

        /// <summary>
        /// Should be used for refreshing accessToken
        /// </summary>
        /// <returns>Access token</returns>
        public string CreateAccessJWT(string refreshToken, out string newRefreshToken)
        {
            m_logger.Info("Creating access token");

            if (!Validate(refreshToken, out int userId))
            {
                newRefreshToken = null;
                return null;
            }

            if (userId == 0)
            {
                newRefreshToken = null;
                return null;
            }

            m_logger.Info($"Creating access token for user {userId}");

            bool revoked = false;
            GlobalSettings.Instance.Database.ExecuteSql(
                @"SELECT COUNT(*) as count
                FROM `session`
                WHERE `user_id` = @p_user_id
                AND `token` = @p_token
                AND `sign_out` = 1",
                reader =>
                {
                    int rowCount = reader.GetInt32("count").Value;

                    if (rowCount > 0)
                    {
                        revoked = true;
                    }
                },
                new Dictionary<string, object>
                {
                    { "p_user_id", userId },
                    { "p_token", refreshToken },
                }
            );

            if (revoked)
            {
                m_logger.Info($"Refresh token is revoked, can't update");
                newRefreshToken = null;
                return null;
            }

            int sessionId = 0;
            GlobalSettings.Instance.Database.ExecuteSql(
                @"SELECT `session_id`
                FROM `session`
                WHERE `user_id` = @p_user_id
                AND `token` = @p_token",
                reader =>
                {
                    sessionId = reader.GetInt32("session_id").Value;
                },
                new Dictionary<string, object>
                {
                    { "p_user_id", userId },
                    { "p_token", refreshToken },
                }
            );

            if (sessionId == 0)
            {
                newRefreshToken = null;
                return null;
            }

            string accessToken = CreateJWT(userId, TimeSpan.FromMinutes(15));
            newRefreshToken = CreateRefreshJWT(userId);

            m_logger.Info($"Updating refresh token for session {sessionId}");

            GlobalSettings.Instance.Database.ExecuteSql(
                @"UPDATE `session`
                SET `token` = @p_new_token
                WHERE `session_id` = @p_session_id",
                null,
                new Dictionary<string, object>
                {
                    { "p_session_id", sessionId },
                    { "p_new_token", newRefreshToken },
                }
            );

            return accessToken;
        }

        public static int GetUserId(IEnumerable<Claim> claims)
        {
            if (claims == null)
            {
                return 0;
            }

            foreach (Claim claim in claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    return int.Parse(claim.Value);
                }
            }

            return 0;
        }

        /// <summary>
        /// Should be executed on authorization
        /// </summary>
        /// <returns>Refresh token</returns>
        public string CreateSession(string deviceName, int userId, string audience = "WebClient")
        {
            m_logger.Info($"Creating new session for user {userId}");
            string refreshToken = CreateRefreshJWT(userId, audience);

            GlobalSettings.Instance.Database.ExecuteSql(
                @"INSERT INTO `session` (`user_id`, `device_name`, `token`)
                VALUES (@p_user_id, @p_device_name, @token)",
                null,
                new Dictionary<string, object>
                {
                    { "p_user_id", userId },
                    { "p_device_name", deviceName },
                    { "token", refreshToken },
                }
            );

            return refreshToken;
        }

        private string CreateRefreshJWT(int userId, string audience = "WebClient")
        {
            return CreateJWT(userId, TimeSpan.FromDays(30), audience);
        }

        public string CreateJWT(int userId, TimeSpan duration, string audience = "WebClient")
        {
            m_logger.Info($"Generating JWT for user {userId}");

            string roles = GetUserRoles(userId);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SecurityToken token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = m_issuer,
                Audience = audience,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.Add(duration),
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Role, roles),
                }),
                SigningCredentials = new SigningCredentials(m_securityKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha512Digest),
            });

            return handler.WriteToken(token);
        }

        private string GetUserRoles(int userId)
        {
            string roles = string.Empty;
            GlobalSettings.Instance.Database.ExecuteSql(
                $@"SELECT `user_id`, `role_id`, `role`
                FROM `user`
                INNER JOIN `user_roles` ON `user_roles`.`user_id` = `user`.`id`
                INNER JOIN `user_role` ON `user_roles`.`role_id` = `user_role`.`id`
                WHERE `user`.`id` = {userId}
                ",
                onRow =>
                {
                    roles += onRow.GetString("role") + ",";
                });
            return roles.TrimEnd(',');
        }

        public bool Validate(string token, out int userId, string audience = "WebClient")
        {
            m_logger.Info($"Validating token");

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidIssuer = m_issuer,
                ValidAudience = audience,
                IssuerSigningKey = m_securityKey,
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                m_logger.Info($"Authenticated = {claimsPrincipal.Identity.IsAuthenticated}");

                userId = JwtHelper.GetUserId(claimsPrincipal.Claims);
                return claimsPrincipal.Identity.IsAuthenticated;

            }
            catch (Exception e)
            {
                m_logger.Info($"Token error");
                m_logger.Error(e);
                userId = 0;
                return false;
            }
        }
    }
}
