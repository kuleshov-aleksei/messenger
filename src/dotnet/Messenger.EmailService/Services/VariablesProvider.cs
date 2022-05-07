using System.Collections.Generic;
using System.Linq;

namespace Messenger.EmailService.Services
{
    public class VariablesProvider
    {
        private readonly Dictionary<string, string> m_exampleData = new Dictionary<string, string>()
        {
            { "{UserFirstName}", "Иван" },
            { "{UserSurname}", "Петров" },
            { "{Username}", "ivan.petrov" },
            { "{UserEmail}", "ivan.petrov@gmail.com" },
            { "{Website}", "https://messenger.local.encamy.com/" },
            { "{CurrentDate}", "2022-05-07 15:28" },
            { "{ActivateAccountLink}", "https://messenger.local.encamy.com/user/activate/c36c2c60-a3a6-44c0-befa-f4ec96b5f041" },
            { "{RestorePasswordLink}", "https://messenger.local.encamy.com/user/restore/e8c9eaea-896a-4f22-aad5-8b58fecdddf4" },
        };

        public List<string> GetVariables()
        {
            return m_exampleData.Keys.ToList();
        }

        public Dictionary<string, string> GetExampleData()
        {
            return m_exampleData;
        }
    }
}
