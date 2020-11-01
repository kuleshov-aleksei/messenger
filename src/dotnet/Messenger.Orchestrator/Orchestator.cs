using Messenger.Common;
using MySql.Common;
using NLog;
using System.Collections.Generic;

namespace Messenger.Orchestrator
{
    internal class Orchestator
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private List<Service> m_services;

        public Orchestator()
        {
            m_services = LoadServicesFromDb();

            m_logger.Info("Loaded services:");
            foreach (Service service in m_services)
            {
                m_logger.Info(service);
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
    }
}
