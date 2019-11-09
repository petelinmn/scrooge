using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Scrooge.DataAccess;

namespace Scrooge.Infrastructure.Configuration
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string GetDefaultConnectionString()
        {
            return _configuration["ConnectionStrings:Default"];
        }

        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration;
    }
}
