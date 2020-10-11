﻿using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using MySql.Common;

namespace Messenger.Common.JWT
{
    public class JwtHelper
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private string m_issuer;
        private SymmetricSecurityKey m_securityKey;

        public JwtHelper(string issuer, string secretKeyPath)
        {
            m_issuer = issuer;

            if (!File.Exists(secretKeyPath))
            {
                throw new ApplicationException("Secret JWT key does not exists");
            }

            byte[] secret = File.ReadAllBytes(secretKeyPath);
            m_securityKey = new SymmetricSecurityKey(secret);
        }

        /// <summary>
        /// Should be used for refreshing accessToken
        /// </summary>
        /// <returns>Access token</returns>
        public string CreateAccessJWT(string refreshToken)
        {
            IEnumerable<Claim> claims;
            if (!Validate(refreshToken, out claims))
            {
                return null;
            }

            int userId = 0;
            foreach (Claim claim in claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    userId = int.Parse(claim.Value);
                }
            }

            if (userId == 0)
            {
                return null;
            }

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
                return null;
            }

            return CreateJWT(userId, TimeSpan.FromMinutes(15));
        }

        /// <summary>
        /// Should be executed on authorization
        /// </summary>
        /// <returns>Refresh token</returns>
        public string CreateRefreshJWT(string deviceName, int userId, string audience = "WebClient")
        {
            string refreshToken = CreateJWT(userId, TimeSpan.FromDays(30), audience);

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

        public string CreateJWT(int userId, TimeSpan duration, string audience = "WebClient")
        {
            m_logger.Info($"Generating JWT for user {userId}");

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
                }),
                SigningCredentials = new SigningCredentials(m_securityKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha512Digest),
            });

            return handler.WriteToken(token);
        }

        public bool Validate(string token, out IEnumerable<Claim> claims, string audience = "WebClient")
        {
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
                claims = claimsPrincipal.Claims;
                return claimsPrincipal.Identity.IsAuthenticated;

            }
            catch (Exception e)
            {
                m_logger.Error(e);
                claims = null;
                return false;
            }
        }
    }
}
