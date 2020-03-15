using System;
using System.Collections.Generic;
using System.Text;
using Scrooge.DataAccess.Models;

namespace Scrooge.DataAccess.Common
{
    public interface IMarketRepository
    {

        /// <summary>
        /// Get list of markets
        /// </summary>
        /// <returns></returns>
        List<Market> GetMarkets();

        /// <summary>
        /// Get list of markets
        /// </summary>
        /// <returns></returns>
        List<MarketInfo> GetMarketsInfo();

        /// <summary>
        /// Save list of markets
        /// </summary>
        /// <param name="assets"></param>
        void Save(List<Market> markets);
    }
}
