using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Common;

namespace Scrooge.Task.Tasks
{
    class InitializeTask : TaskBase
    {
        protected override void Execute()
        {
            var service = Container.GetService<IDataCommonService>();
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
