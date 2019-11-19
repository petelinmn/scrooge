using System;
using System.Threading.Tasks;
using Scrooge.BusinessLogic.Collector;
using Microsoft.Extensions.DependencyInjection;

namespace Scrooge.Exchange.DataCollector
{
    public class BinanceCollector : BaseCollector
    {
        public override void Execute()
        {
            var service = Container.GetService<IDataCollectorService>();
            using (var uow = GetUnitOfWork())
            {
                var isAddedNewMarkets = service.MarketCollectionInitialize().Result;

                uow.Commit();

                if (isAddedNewMarkets)
                    Log.Information("List of markets was updated");
            }
        }
    }
}
