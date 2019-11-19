
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Scrooge.Exchange.Connectors.Models;

namespace Scrooge.Exchange.Connectors.BinanceConnector
{
    public partial class BinanceConnector
    {
        public async Task<bool> Ping()
        {
            var response = await SendRequest<JObject>("ping", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
            return response.Type == JTokenType.Object;
        }

        public async Task<IList<PriceInfo>> TickerAllPrices()
        {
            return await SendRequest<IList<PriceInfo>>("ticker/allPrices", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
        }
    }
}
