using System;
using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrooge.Task.MigrationsTasks.Migrations;

namespace Scrooge.Task.MigrationsTasks.Tasks
{
    public abstract class MigrateBaseTask : TaskBase
    {
        protected static IServiceProvider ServiceProvider;

        protected override void CreateServices()
        {
            var connectionString = Configuration["ConnectionStrings:Default"];

            ServiceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    rb.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Migration201911261647).Assembly).For.Migrations();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}
