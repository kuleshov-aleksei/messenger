using NLog;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Messenger.JWTSecret
{
    class Program
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            m_logger.Info("*** Starting ***");

            byte[] buffer = new byte[512];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(buffer);
            }
            string secretString = Convert.ToBase64String(buffer);

            if (!Directory.Exists("key"))
            {
                Directory.CreateDirectory("key");
            }

            string path = Path.Combine("key", "jwt_secret.secret");
            File.WriteAllText(path, secretString);
            m_logger.Info($"Wrote key to {path}");
        }
    }
}
