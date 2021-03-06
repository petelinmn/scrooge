﻿using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Common;

namespace Scrooge.Exchange.DataCollector
{
    public class BinanceCollector : BaseCollector
    {
        public override void Execute()
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
