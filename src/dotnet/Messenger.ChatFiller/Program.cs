using System;
using System.Threading.Tasks;

namespace Messenger.ChatFiller
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string chatCreatorToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJ1c2VyX2lkIjoiNSIsIm5iZiI6MTY1MDEyMjE3NiwiZXhwIjoxNjUyNzE0MTc2LCJpYXQiOjE2NTAxMjIxNzYsImlzcyI6IkF1dGhTZXJ2ZXIiLCJhdWQiOiJXZWJDbGllbnQifQ.BThXLbtG7EvoOQjLU3sB-nyMIXNGDG8y4ajIMZ8F_iM";

            MessengerApi messengerApi = new MessengerApi();
            TranscriptFiller transcriptFiller = new TranscriptFiller("scripts", messengerApi, chatCreatorToken);
            await transcriptFiller.Process();
        }
    }
}