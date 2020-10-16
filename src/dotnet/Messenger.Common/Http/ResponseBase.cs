using Newtonsoft.Json;

namespace Messenger.Common.Http
{
    public class ResponseBase
    {
        public string ToJson(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }
    }
}
