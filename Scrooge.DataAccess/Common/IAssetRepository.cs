using System;
using System.Collections.Generic;
using System.Text;
using Scrooge.DataAccess.Models;

namespace Scrooge.DataAccess.Common
{
    public interface IAssetRepository
    {
        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns></returns>
        IEnumerable<Asset> GetAssets();

        /// <summary>
        /// Get list of markets
        /// </summary>
        /// <returns></returns>
        IEnumerable<Market> GetMarkets();

        /// <summary>
        /// Save list of assets
        /// </summary>
        /// <param name="assets"></param>
        void Save(List<Asset> assets);

        /// <summary>
        /// Save list of markets
        /// </summary>
        /// <param name="assets"></param>
        void Save(List<Market> markets);
    }
}
