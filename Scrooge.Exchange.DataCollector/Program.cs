using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Scrooge.Exchange.DataCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            var collectorTypes = Assembly.GetCallingAssembly().GetTypes()
                .Where(type =>
                    typeof(BaseCollector).IsAssignableFrom(type)
                    && !type.IsAbstract
                    && type.GetCustomAttribute<ObsoleteAttribute>() == null).ToList();

            foreach (var type in collectorTypes)
                Activator.CreateInstance(type);
        }
    }
}
