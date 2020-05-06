using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Common;
using Scrooge.DataAccess.Util;
using System;
using System.Threading;

namespace Scrooge.Task.Tasks
{
    class CollectorTask : TaskBase
    {
        protected override void Execute()
        {
            try
            {                
                var service = Container.GetService<IDataCommonService>();
                var checkEach = 10;
                var delta = 5;
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

        void Step(IUnitOfWork uow, IDataCommonService dataService, DateTime requestDate)
        {
            Console.WriteLine("Begin1");
            uow.Begin();
            Console.WriteLine("Begin2");
            try
            {
                dataService.Collect(requestDate);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}\n{e.StackTrace}");
                Thread.Sleep(1000);
            }
            Console.WriteLine("End1");
            uow.Commit();
            Console.WriteLine("End2");
        }
    }
}
