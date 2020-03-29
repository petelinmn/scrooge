﻿using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Common;
using System;
using System.Threading;

namespace Scrooge.Task.Tasks
{
    class CollectorTask : TaskBase
    {
        protected override void Execute()
        {
            var service = Container.GetService<IDataCommonService>();
            using (var uow = GetUnitOfWork())
            {
                DateTime runDate; 
                while (true)
                {
                    uow.Begin();
                    runDate = DateTime.Now;
                    var isAddedNewMarkets = service.Collect().Result;

                    uow.Commit();

                    if (isAddedNewMarkets)
                        Log.Information("List of markets was updated");
                    Thread.Sleep(5000 - (int)(DateTime.Now-runDate).TotalMilliseconds);
                }
                
            }
        }
    }
}
