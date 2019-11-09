using Microsoft.Extensions.DependencyInjection;
using Scrooge.Infrastructure.Installers.Application;

namespace Scrooge.Infrastructure.Installers
{
    public static class CollectorsInstaller
    {
        public static void AddCollectorsDependencies(this IServiceCollection services)
        {
            services.AddDataAccessCommon();
        }
    }
}
