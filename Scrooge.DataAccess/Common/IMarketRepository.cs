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
        IEnumerable<Market> GetMarkets(bool onlyActive = true);

        /// <summary>
        /// Get list of markets
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketInfo> GetMarketsInfo();

        /// <summary>
        /// Save list of markets
        /// </summary>
        /// <param name="markets"></param>
        void Save(IReadOnlyCollection<Market> markets);
    }
}
