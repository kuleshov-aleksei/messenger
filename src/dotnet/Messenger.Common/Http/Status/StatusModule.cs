using EmbedIO;
using Humanizer;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Messenger.Common.Http.Status
{
    class StatusModule : ModuleBase<StatusRequest>
    {
        public override bool IsFinalHandler => true;
        private MemoryMetricsClient m_memoryClient;

        private string m_version;
        private string m_name => Assembly.GetEntryAssembly().GetName().Name;
        private string m_machineName => Environment.MachineName;
        private string m_osVersion => $"{Environment.OSVersion} {(Environment.Is64BitOperatingSystem ? "x64" : string.Empty)}";
        private int m_processorsCount => Environment.ProcessorCount;
        private string m_userName => Environment.UserDomainName;
        private string m_startTime => Process.GetCurrentProcess().StartTime.ToString("HH:mm:ss dd.MM.yyyy");
        private string m_workingTime => (DateTime.Now - Process.GetCurrentProcess().StartTime).Humanize();



        public StatusModule()
            : base (Routes.STATUS_ROUTE, null)
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            m_version = $"{version.Major}.{version.Minor}.{version.Build}";
            m_memoryClient = new MemoryMetricsClient();
        }

        protected override async Task OnRequest(IHttpContext context, StatusRequest request, int userId)
        {
            MemoryMetrics metrics = m_memoryClient.GetMetrics();

            await SendResponse(context, HttpStatusCode.OK,
                JsonConvert.SerializeObject(
                new StatusResponse
                {
                    Version = m_version,
                    Name = m_name,
                    MachineName = m_machineName,
                    OSVersion = m_osVersion,
                    ProcessorCount = m_processorsCount,
                    UserName = m_userName,
                    UsedMemory = metrics.Used.Megabytes().Humanize("0.00"),
                    TotalMemory = metrics.Total.Megabytes().Humanize("0.00"),
                    FreeMemory = metrics.Free.Megabytes().Humanize("0.00"),
                    StartTime = m_startTime,
                    WorkingTime = m_workingTime,
                },
                Formatting.Indented)
            );
        }
    }
}
