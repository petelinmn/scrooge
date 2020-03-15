using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Common;
using Scrooge.BusinessLogic.Implement.Common;
using Scrooge.DataAccess.Common;
using Scrooge.DataAccess.Repository.Common;
using Scrooge.Exchange.Connectors;
using Scrooge.Exchange.Connectors.BinanceConnector;
using Scrooge.Infrastructure.Installers.Application;

namespace Scrooge.Infrastructure.Installers
{
    public static class TasksInstaller
    {
        public static void AddTasksDependencies(this IServiceCollection services)
        {
            services.AddDataAccessCommon();
            services.AddScoped<IDataCommonService, DataCommonService>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IMarketRepository, MarketRepository>();
            services.AddScoped<IConnector, BinanceConnector>();
        }
    }
}
