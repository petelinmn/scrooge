
using System.Collections.Generic;
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

        public async Task<IEnumerable<PriceInfo>> TickerAllPrices()
        {
            return await SendRequest<List<PriceInfo>>("ticker/allPrices", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
        }


        public class PriceInfo
        {
            public string Symbol { get; set; }
            public decimal Price { get; set; }

            public override string ToString()
            {
                return $"{Symbol}: {Price}";
            }
        }
    }
}
