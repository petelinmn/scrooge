using Scrooge.BusinessLogic.Collector;
using Microsoft.Extensions.DependencyInjection;

namespace Scrooge.Exchange.DataCollector
{
    public class BinanceCollector : BaseCollector
    {
        public override void Execute()
        {
            Log.Information("Executing successful");
            var service = Container.GetService<IDataCollectorService>();
            using (var uow = GetUnitOfWork())
            {
                service.MarketDefinition();
                uow.Commit();
            }
        }
    }
}
