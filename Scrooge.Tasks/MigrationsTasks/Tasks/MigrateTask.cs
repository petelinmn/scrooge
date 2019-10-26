
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Scrooge.Task.MigrationsTasks.Tasks
{
    class MigrateTask : MigrateBaseTask
    {
        public override void Execute(string[] args)
        {
            using var scope = ServiceProvider.CreateScope();
            ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
        }
    }
}
