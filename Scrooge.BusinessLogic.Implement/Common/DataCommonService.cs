using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scrooge.Exchange.Connectors;
using Scrooge.DataAccess.Common;
using Scrooge.DataAccess.Models;
using Scrooge.Exchange.Connectors.Models;
using Scrooge.BusinessLogic.Common;

namespace Scrooge.BusinessLogic.Implement.Common
{
    public class DataCommonService : IDataCommonService
    {
        /// <summary>
        /// First initialize collections of assets and markets
        /// </summary>
        /// <returns></returns>
        public async Task<bool> MarketCollectionInitialize()
        {
            var tickerResult = await _binanceConnector.TickerAllPrices();

            var allAssets = _assetRepository.GetAssets();

            var assetMarketMap = GetAssetMarketMap(allAssets, tickerResult);

            return false;
        }

        /// <summary>
        /// Get mapping dictionary for asset and collection its markets
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="tickerResult"></param>
        /// <returns></returns>
        private static Dictionary<Asset, List<string>> GetAssetMarketMap(IEnumerable<Asset> assets, IList<PriceInfo> tickerResult)
        {
            var primaryAssets = assets.Where(i => i.IsMain);
            
            var assetMarketMap = new Dictionary<Asset, List<string>>();
            foreach (var asset in primaryAssets)
            {
                var assetMarkets = tickerResult.ToList().Where(i => i.Symbol.EndsWith(asset.Name)).Select(i => i.Symbol).OrderBy(i => i).ToList();
                assetMarketMap.Add(asset, assetMarkets);
            }

            return assetMarketMap;
        }

        /// <summary>
        /// Return second asset from market
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="market"></param>
        /// <returns></returns>
        private static string GetSecondAsset(string asset, string market)
        {
            System.Diagnostics.Debug.Assert(asset != null && market != null);

            return market.StartsWith(asset)
                ? market.Substring(asset.Length, market.Length - asset.Length)
                : market.Substring(0, market.Length - asset.Length);
        }

        public DataCommonService(IAssetRepository assetRepository, IConnector binanceConnector)
        {
            _assetRepository = assetRepository;
            _binanceConnector = binanceConnector;
        }

        private readonly IAssetRepository _assetRepository;
        private readonly IConnector _binanceConnector;
    }
}
