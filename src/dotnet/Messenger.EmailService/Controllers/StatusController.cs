using Humanizer;
using Messenger.Common.Http.Status;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Messenger.EmailService.Controllers
{
    [ApiController]
    public class StatusController : ControllerBase
    {
        private MemoryMetricsClient m_memoryClient;

        private string m_version;
        private string m_name => Assembly.GetEntryAssembly().GetName().Name;
        private string m_machineName => Environment.MachineName;
        private string m_osVersion => $"{Environment.OSVersion} {(Environment.Is64BitOperatingSystem ? "x64" : string.Empty)}";
        private int m_processorsCount => Environment.ProcessorCount;
        private string m_userName => Environment.UserDomainName;
        private string m_startTime => Process.GetCurrentProcess().StartTime.ToString("HH:mm:ss dd.MM.yyyy");
        private string m_workingTime => (DateTime.Now - Process.GetCurrentProcess().StartTime).Humanize(culture: new System.Globalization.CultureInfo("ru-RU"));

        public StatusController()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            m_version = $"{version.Major}.{version.Minor}.{version.Build}";
            m_memoryClient = new MemoryMetricsClient();
        }

        [HttpGet("status")]
        [Produces("application/json")]
        public StatusResponse GetStatus()
        {
            MemoryMetrics metrics = m_memoryClient.GetMetrics();

            return new StatusResponse
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
                UsedMemoryPercent = (int)Math.Round(metrics.Used / metrics.Total * 100),
                StartTime = m_startTime,
                WorkingTime = m_workingTime,
            };
        }
    }
}
