namespace Messenger.SubscribingService.Models
{
    public enum ErrorReasons
    {
        NoError = 0,
        TooManyDevicesConnected,
    }

    public static class ErrorReasonsEnumExtensions
    {
        public static string DescibeErrorReason(this ErrorReasons errorReason)
        {
            switch (errorReason)
            {
                case ErrorReasons.TooManyDevicesConnected: return "Too many devices connected";
                case ErrorReasons.NoError: return "No error";
                default: return "Unknown error";
            }
        }
    }
}
