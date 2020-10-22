using Grpc.Core;
using NLog;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.gRPC
{
    internal class EchoServiceImpl : EchoService.EchoServiceBase
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public override Task<EchoResponse> Echo(EchoRequest request, ServerCallContext context)
        {
            m_logger.Info("Got request echo: " + request.Message);

            return Task.FromResult(new EchoResponse
            {
                Message = "echo " + request.Message
            });
        }
    }
}
