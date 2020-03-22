using System;
using System.Collections.Generic;
using System.Text;
using Scrooge.DataAccess.Models;

namespace Scrooge.DataAccess.Common
{
    public interface IPriceRepository
    {
        
        List<Price> GetPrices();

        List<Price> GetLastPrices();

        void Save(List<Price> prices);
    }
}
