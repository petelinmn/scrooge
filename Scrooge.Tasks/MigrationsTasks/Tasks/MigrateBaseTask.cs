using System;
using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrooge.Task.MigrationsTasks.Migrations;

namespace Scrooge.Task.MigrationsTasks.Tasks
{
    public class MigrateBaseTask : TaskBase
    {
        protected static IServiceProvider ServiceProvider;
        public MigrateBaseTask()
        {
            ServiceProvider = CreateServices();
        }

        private IServiceProvider CreateServices()
        {
            var connectionString = Configuration["ConnectionStrings:Default"];

            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    rb.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Migration201910261354).Assembly).For.Migrations();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}
