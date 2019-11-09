
using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Scrooge.Task.MigrationsTasks.Tasks
{
    class RollbackTask : MigrateBaseTask
    {
        protected override void Execute()
        {
            Log.Information("Task started");
            try
            {
                using var scope = ServiceProvider.CreateScope();
                ServiceProvider.GetRequiredService<IMigrationRunner>().Rollback(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return;
            }
            Log.Information("Task was executed successful");
        }
    }
}
