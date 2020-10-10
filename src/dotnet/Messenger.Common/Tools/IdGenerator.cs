using System;
using System.Security.Cryptography;

namespace Messenger.Common.Tools
{
    public class IdGenerator
    {
        private byte m_localCounter = 0;
        private ulong m_randomPart = 0;
        private object m_lock;

        public IdGenerator()
        {
            m_lock = new object();

            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] dataToFill = new byte[1];
            random.GetBytes(dataToFill);

            ulong randomNumber = (ulong)dataToFill[0];
            m_randomPart = (randomNumber << 16) & 0x0000_0000_FFFF_0000;
        }

        public ulong GenerateUniqueId()
        {
            lock (m_lock)
            {
                ulong id = GenerateEpochPart();
                id |= m_randomPart;
                id |= GenerateCounterPart();

                return id;
            }
        }

        private ulong GenerateEpochPart()
        {
            ulong epoch = (ulong)UnixEpochTools.ToEpoch(DateTime.UtcNow);
            return (epoch << 32) & 0xFFFF_FFFF_0000_0000;
        }

        private ulong GenerateCounterPart()
        {
            ulong counter = m_localCounter++;
            return counter & 0x0000_0000_0000_FFFF;
        }
    }
}
