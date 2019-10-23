
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Scrooge.Exchange.Connectors.BinanceConnector
{
    public partial class BinanceConnector
    {
        public async Task<bool> Ping()
        {
            var response = await SendRequest<JObject>("ping", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
            return response.Type == JTokenType.Object;
        }
    }
}
