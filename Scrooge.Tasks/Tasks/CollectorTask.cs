using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Common;
using System.Threading;

namespace Scrooge.Task.Tasks
{
    class CollectorTask : TaskBase
    {
        protected override void Execute()
        {
            var service = Container.GetService<IDataCommonService>();
            while (true)
            {
                using (var uow = GetUnitOfWork())
                {
                    var isAddedNewMarkets = service.Collect().Result;

                    uow.Commit();

                    if (isAddedNewMarkets)
                        Log.Information("List of markets was updated");
                }
                Thread.Sleep(5000);
            }
        }
    }
}
