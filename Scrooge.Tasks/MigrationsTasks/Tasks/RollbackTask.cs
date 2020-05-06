
using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Scrooge.Task.MigrationsTasks.Tasks
{
    class RollbackTask : MigrateBaseTask
    {
        protected override void Execute()
        {            
            try
            {
                Log.Information("Rollback task is started");
                using var scope = ServiceProvider.CreateScope();
                ServiceProvider.GetRequiredService<IMigrationRunner>().Rollback(1);
                Log.Information("Rollback task was executed successful");
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}\n{e.StackTrace}");
                return;
            }            
        }
    }
}
