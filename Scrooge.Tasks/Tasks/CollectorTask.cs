using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Common;
using Scrooge.DataAccess.Util;
using System;
using System.Threading;

namespace Scrooge.Task.Tasks
{
    internal class CollectorTask : TaskBase
    {
        protected override void Execute()
        {
            try
            {                
                var service = Container.GetService<IDataCommonService>();
                const int checkEach = 10;
                const int delta = 5;
                var lastSec = 1;

                Log.Information("Collector task is started");
                var uow = GetUnitOfWork();
                {
                    while (true)
                    {
                        var currentMoment = DateTime.UtcNow;
                        if (currentMoment.Second != lastSec && currentMoment.Second % delta == 0)
                        {
                            lastSec = currentMoment.Second;
                            Step(uow, service, currentMoment);
                        }

                        Thread.Sleep(checkEach);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}\n{e.StackTrace}");
            }
        }

        private void Step(IUnitOfWork uow, IDataCommonService dataService, DateTime requestDate)
        {
            uow.Begin();

            try
            {
                dataService.Collect(requestDate);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}\n{e.StackTrace}");
                Thread.Sleep(1000);
            }

            uow.Commit();
        }
    }
}
