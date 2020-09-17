using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Scrooge.DataAccess;

namespace Scrooge.Infrastructure.Configuration
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        IConfiguration Configuration { get; }
        public string GetDefaultConnectionString()
        {
            return Configuration["ConnectionStrings:Default"];
        }

        public ConnectionStringProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
