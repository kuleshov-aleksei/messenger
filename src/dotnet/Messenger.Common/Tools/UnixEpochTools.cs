using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Common.Tools
{
    public static class UnixEpochTools
    {
        public static int ToEpoch(DateTime time)
        {
            return (int)time.Subtract(DateTime.UnixEpoch).TotalSeconds;
        }
    }
}
