using Newtonsoft.Json;

namespace Messenger.EmailService.Models
{
    public class SetTemplateModel
    {
        [JsonProperty("template")]
        public string Template { get; set; }
    }
}
