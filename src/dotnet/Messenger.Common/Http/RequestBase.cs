using Newtonsoft.Json;

namespace Messenger.Common.Http
{
    public abstract class RequestBase : IRequest
    {
        public string ToJson(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public abstract bool Validate();
    }
}
