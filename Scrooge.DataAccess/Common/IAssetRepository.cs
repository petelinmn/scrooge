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
        List<Asset> GetAssets();

        /// <summary>
        /// Save list of assets
        /// </summary>
        /// <param name="assets"></param>
        void Save(List<Asset> assets);
    }
}
