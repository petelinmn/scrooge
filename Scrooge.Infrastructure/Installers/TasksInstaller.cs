using Microsoft.Extensions.DependencyInjection;
using Scrooge.Infrastructure.Installers.Application;

namespace Scrooge.Infrastructure.Installers
{
    public static class TasksInstaller
    {
        public static void AddTasksDependencies(this IServiceCollection services)
        {
            services.AddDataAccessCommon();
        }
    }
}
