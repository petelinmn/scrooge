using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var primaryAssets = assets.ToDictionary(i=>i, i=>tickerResult.Where(t=>t.Symbol.EndsWith(i.Name)).Select(s => s.Symbol).ToList());
            Dictionary<string, int> text = new Dictionary<string, int>();
            foreach (var primAss in primaryAssets)
            {
                foreach (var primAssValue in primAss.Value)
                {
                    var str = primAssValue.Replace(primAss.Key.Name, " ");
                    if (text.ContainsKey(str))
                        text[str]++;
                    else
                        text.Add(str, 1);
                }
            }
            var fourValue = text.Where(i => i.Value == assets.Count()).ToList();
            var assetMarketMap = assets.ToDictionary(i => i, i => tickerResult.Where(t => fourValue.Any(f=>f.Key + i.Name == t.Symbol)).Select(s => s.Symbol).ToList());
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
