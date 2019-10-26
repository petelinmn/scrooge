using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Scrooge.Task
{
    public class TaskBase : IDisposable
    {
        protected TaskBase()
        {
            CreateAppConfiguration();
        }

        public virtual void Execute(string[] args) { }

        protected IConfiguration Configuration { get; private set; }
        private void CreateAppConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void Dispose()
        {
            Configuration = null;
        }
    }
}
