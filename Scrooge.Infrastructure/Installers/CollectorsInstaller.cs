using Microsoft.Extensions.DependencyInjection;
using Scrooge.BusinessLogic.Collector;
using Scrooge.BusinessLogic.Implement.Collector;
using Scrooge.DataAccess;
using Scrooge.DataAccess.Common;
using Scrooge.DataAccess.Repository;
using Scrooge.DataAccess.Repository.Common;
using Scrooge.Infrastructure.Installers.Application;

namespace Scrooge.Infrastructure.Installers
{
    public static class CollectorsInstaller
    {
        public static void AddCollectorsDependencies(this IServiceCollection services)
        {
            services.AddDataAccessCommon();

            services.AddScoped<IDataCollectorService, DataCollectorService>();

            services.AddScoped<IAssetRepository, AssetRepository>();
        }
    }
}
