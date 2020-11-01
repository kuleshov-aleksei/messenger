using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.Orchestrator.StatusModule
{
    internal class ServiceStatusModule : ModuleBase<ServiceStatusRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private Orchestator m_orchestator;

        public override bool IsFinalHandler => true;

        public ServiceStatusModule(Orchestator orchestator, JwtHelper jwtHelper)
            : base (Routes.ServiceStatus, jwtHelper)
        {
            m_orchestator = orchestator;
        }

        protected override async Task OnRequest(IHttpContext context, ServiceStatusRequest request, int userId)
        {
            m_logger.Info("Handling service status request");

            List<Service> services = m_orchestator.GetServices();
            ServiceStatusResponse response = new ServiceStatusResponse
            {
                Services = services
            };

            await SendResponse(context, HttpStatusCode.OK, response);
        }
    }
}
