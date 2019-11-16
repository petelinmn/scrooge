using System.Collections.Generic;
using System.Threading.Tasks;
using Scrooge.Exchange.Connectors.Models;

namespace Scrooge.Exchange.Connectors
{
    public interface IConnector
    {
        Task<bool> Ping();

        Task<IEnumerable<PriceInfo>> TickerAllPrices();
    }
}
