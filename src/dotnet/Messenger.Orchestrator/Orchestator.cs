using Messenger.Common.Settings;
using MySql.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Messenger.Orchestrator
{
    internal class Orchestator : IDisposable
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private List<Service> m_services;
        private Thread m_thread;
        private volatile bool m_running;
        private TimeSpan m_interval = TimeSpan.FromSeconds(20);

        public Orchestator()
        {
            m_services = LoadServicesFromDb();

            m_logger.Info("Loaded services:");
            foreach (Service service in m_services)
            {
                m_logger.Info(service);
            }

            m_thread = new Thread(ThreadFunc);
            m_thread.IsBackground = true;
        }

        public void Start()
        {
            m_thread.Start();
        }

        public void Stop()
        {
            if (m_running)
            {
                m_running = false;
                m_thread.Join();
            }
        }

        public List<Service> GetServices()
        {
            return m_services;
        }

        private void ThreadFunc(object obj)
        {
            m_running = true;

            while (m_running)
            {
                LoadStatus();

                Thread.Sleep(m_interval);
            }
        }

        private void LoadStatus()
        {
            foreach (Service service in m_services)
            {
                service.GetStatus();
            }
        }

        private List<Service> LoadServicesFromDb()
        {
            List<Service> services = new List<Service>();

            GlobalSettings.Instance.Database.ExecuteSql(
                @"SELECT `services`.`title`, `services`.`description`, `services`.`address`, `settings`.`value` AS 'port'
                FROM `services` 
                INNER JOIN `settings` ON `services`.`settings_port_id` = `settings`.`id`;",
                reader =>
                {
                    services.Add(new Service
                    {
                        Address = reader.GetString("address"),
                        Description = reader.GetString("description"),
                        Name = reader.GetString("title"),
                        Port = reader.GetInt32("port").Value
                    });
                }
            );

            return services;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
