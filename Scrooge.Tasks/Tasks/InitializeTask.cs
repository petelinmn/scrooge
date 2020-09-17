using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Common;
using System;

namespace Scrooge.Task.Tasks
{
    class InitializeTask : TaskBase
    {
        protected override void Execute()
        {
            try
            {
                Log.Information("Initialize task is started");
                var service = Container.GetService<IDataCommonService>();
                using (var uow = GetUnitOfWork())
                {
                    service.MarketCollectionInitialize().Wait();
                    uow.Commit();
                }

                Log.Information("Initialize task was executed successful");
            }
            catch(Exception e)
            {
                Log.Error($"{e.Message}\n{e.StackTrace}");
            }   
        }
    }
}
