using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Common.Tools
{
    public static class UnixEpochTools
    {
        public static long ToEpoch(DateTime time)
        {
            return (long)time.Subtract(DateTime.UnixEpoch).TotalMilliseconds;
        }
    }
}
