using System;
using Scrooge.Exchange.Connectors;
using  Scrooge.Exchange.Connectors.BinanceConnector;

namespace Scrooge.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnector bc = new BinanceConnector();
            var pingResult = bc.Ping().Result;
            
            var tickerResult = bc.TickerAllPrices().Result;

            Console.WriteLine($"Ping result:{pingResult}");
        }
    } 
}

