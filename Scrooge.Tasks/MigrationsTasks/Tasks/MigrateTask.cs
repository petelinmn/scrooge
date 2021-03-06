﻿
using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Scrooge.Task.MigrationsTasks.Tasks
{
    class MigrateTask : MigrateBaseTask
    {
        protected override void Execute()
        {            
            try
            {
                Log.Information("Migrate task is started");
                using var scope = ServiceProvider.CreateScope();
                ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
                Log.Information("Migrate task was executed successful");
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}\n{e.StackTrace}");
                return;
            }
        }
    }
}
