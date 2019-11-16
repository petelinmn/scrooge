using Microsoft.Extensions.DependencyInjection;
using Scrooge.DataAccess;
using Scrooge.DataAccess.Util;
using Scrooge.DataAccess.Repository.Util;
using Scrooge.DataAccess.Repository;
using Scrooge.Infrastructure.Configuration;

namespace Scrooge.Infrastructure.Installers.Application
{
    public static class CommonInstaller
    {
        public static void AddDataAccessCommon(this IServiceCollection services)
        {
            services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddScoped<IConnectionProvider, ConnectionProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
