using System;
using System.IO;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrooge.DataAccess.Repository.Util;
using Scrooge.DataAccess.Util;
using Scrooge.Infrastructure.Installers;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;

using Serilog.Events;
using TelegramSink;
using TelegramSink.TelegramBotClient.Domain;

namespace Scrooge.Task
{
    public abstract class TaskBase : IDisposable
    {
        protected TaskBase()
        {
            CreateAndRunApplication();
        }

        protected void CreateAndRunApplication()
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
            services.AddTasksDependencies();

            Container = services.BuildServiceProvider();
        }

        private void CreateLogger()
        {
            var logConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Async(l => l.File(@$"logs/{this.GetType().Name}/{DateTime.Now.ToString("yyyy-MM-dd")}.log"))
                .WriteTo.Async(l => l.Console());

            var isRequiredTelegramLogging = false;
            if (bool.TryParse(Configuration["Telegram:EnabledLogging"], out isRequiredTelegramLogging)
                && isRequiredTelegramLogging)
            {
                var key = Configuration["Telegram:ApiKey"];
                var chats = Configuration["Telegram:Chats"];
                var chatIds = chats.Split(new char[] { ';' });
                foreach(var chatId in chatIds)
                {
                    logConfiguration = logConfiguration
                    .MinimumLevel.Information()
                    .WriteTo.Async(l => l.TeleSink(
                        telegramApiKey: key,
                        telegramChatId: chatId));
                }
            }

            Log = logConfiguration.CreateLogger();
        }

        protected IUnitOfWork GetUnitOfWork()
        {
            IUnitOfWork unitOfWork = Container.GetService<IUnitOfWork>();
            unitOfWork.Begin();
            return unitOfWork;
        }
        protected virtual void CreateServices()
        {

        }
        protected abstract void Execute();

        protected IConfiguration Configuration { get; private set; }
        protected ServiceProvider Container { get; private set; }
        protected Logger Log { get; private set; }

        public void Dispose()
        {
            Configuration = null;
            Container.Dispose();
            Log.Dispose();
        }
    }
}

