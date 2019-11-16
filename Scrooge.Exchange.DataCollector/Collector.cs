
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrooge.DataAccess.Util;
using Scrooge.Infrastructure.Installers;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using TelegramSink;

namespace Scrooge.Exchange.DataCollector
{
    public abstract class BaseCollector : IDisposable
    {
        protected BaseCollector()
        {
            CreateAndRunApplication();
        }
        
        private void CreateAndRunApplication()
        {
            CreateContainer();
            CreateLogger();
            CreateServices();

            Execute();
        }

        private void CreateContainer()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton(Configuration);
            services.AddCollectorsDependencies();

            Container = services.BuildServiceProvider();
        }

        protected IUnitOfWork GetUnitOfWork()
        {
            IUnitOfWork unitOfWork = Container.GetService<IUnitOfWork>();
            unitOfWork.Begin();
            return unitOfWork;
        }

        private void CreateLogger()
        {
            var logConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File($"logs/{this.GetType().Name}/{DateTime.Now.ToString("yyyy-MM-dd")}.log");

            var isRequiredTelegramLogging = false;
            if (isRequiredTelegramLogging)
            {
                logConfiguration = logConfiguration.WriteTo.TeleSink(
                    telegramApiKey: "my-bot-api-key",
                    telegramChatId: "the target chat id");
            }

            Log = logConfiguration.CreateLogger();
        }

        public abstract void Execute();

        protected void CreateServices()
        {

        }

        protected ServiceProvider Container { get; private set; }
        protected Logger Log { get; set; }
        protected IConfiguration Configuration { get; private set; }
        public void Dispose()
        {
            Configuration = null;
            Container.Dispose();
            Log.Dispose();
        }
    }
}
