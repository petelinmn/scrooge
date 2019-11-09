using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Scrooge.Exchange.DataCollector
{
    public class BinanceCollector : BaseCollector
    {
        public override void Execute()
        {
              Log.Information("Executing successful");
        }
    }
}
