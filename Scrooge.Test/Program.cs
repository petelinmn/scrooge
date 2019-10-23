using System;
using  Scrooge.Exchange.Connectors.BinanceConnector;

namespace Scrooge.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BinanceConnector bc = new BinanceConnector();
            var pingResult = bc.Ping().Result;

            Console.WriteLine($"Ping result:{pingResult}");
        }
    } 
}
