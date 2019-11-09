using System;
using Scrooge.Exchange.Connectors;
using Scrooge.Exchange.Connectors.BinanceConnector;
using System.Net;

namespace Scrooge.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var wb = new WebClient();
            string response = wb.DownloadString("https://api.telegram.org/bot973935382:AAHlCvrGDVXF2VkwQe1vszxKj4eN07ZEKxk/getUpdates");

            IConnector bc = new BinanceConnector();
            var pingResult = bc.Ping().Result;
            
            var tickerResult = bc.TickerAllPrices().Result;

            Console.WriteLine($"Ping result:{pingResult}");
        }
    } 
}

