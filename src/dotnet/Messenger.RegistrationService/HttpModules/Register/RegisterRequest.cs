using Messenger.Common.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.RegistrationService.HttpModules.Register
{
    public class RegisterRequest : RequestBase
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Surname))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Email))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }

            return true;
        }
    }
}
