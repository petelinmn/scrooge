using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scrooge.BusinessLogic.Collector;
using Scrooge.Exchange.Connectors;
using Scrooge.DataAccess.Common;
using Scrooge.DataAccess.Models;
using Scrooge.Exchange.Connectors.Models;

namespace Scrooge.BusinessLogic.Implement.Collector
{
    public class DataCollectorService : IDataCollectorService
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

            ReduceToCommon(assetMarketMap);
            
            var newAssets = GetNewAssets(assetMarketMap, allAssets);

            _assetRepository.Save(newAssets);
            allAssets.AddRange(newAssets);

            var newMarkets = GetNewMarkets(allAssets, tickerResult);
            _assetRepository.Save(newMarkets);

            return newMarkets.Count > 0;
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
        /// Get new markets
        /// </summary>
        /// <param name="allAssets"></param>
        /// <param name="tickerResult"></param>
        /// <returns></returns>
        private List<Market> GetNewMarkets(IReadOnlyCollection<Asset> allAssets, IEnumerable<PriceInfo> tickerResult)
        {
            var allMarkets = _assetRepository.GetMarkets();
            var newMarkets = new List<Market>();
            
            foreach (var ticker in tickerResult)
            {
                if (allMarkets.Any(i => i.Name.Equals(ticker.Symbol)))
                    continue;

                foreach (var asset in allAssets)
                {
                    if (!ticker.Symbol.StartsWith(asset.Name))
                        continue;

                    if(newMarkets.Any(i => i.Name.Equals(ticker.Symbol)))
                        continue;
                    
                    var secondAssetName = ticker.Symbol.Substring(asset.Name.Length,
                        ticker.Symbol.Length - asset.Name.Length);

                    if(allAssets.Any(i => i.Name.Equals(secondAssetName)))
                        continue;

                    var secondAsset = allAssets.FirstOrDefault(a => a.Name == secondAssetName);
                    if (secondAsset != null)
                        newMarkets.Add(new Market()
                        {
                            Name = ticker.Symbol, 
                            AssetId1 = asset.Id, 
                            AssetId2 = secondAsset.Id
                        });
                }
            }

            return newMarkets;
        }

        /// <summary>
        /// Get new assets
        /// </summary>
        /// <param name="assetMarketMap"></param>
        /// <param name="assets"></param>
        /// <returns></returns>
        private static List<Asset> GetNewAssets(Dictionary<Asset, List<string>> assetMarketMap, IReadOnlyCollection<Asset> assets)
        {
            var newAssets = new List<Asset>();
            foreach (var item in assetMarketMap)
            {
                var markets = item.Value;
                var asset = item.Key;
                foreach (var market in markets)
                {
                    var secondAsset = GetSecondAsset(asset.Name, market);
                    if (!newAssets.Any(i => i.Name.Equals(secondAsset)) && !assets.Any(i => i.Name.Equals(secondAsset)))
                        newAssets.Add(new Asset()
                        {
                            Name = secondAsset,
                            IsMain = false,
                            IsStable = false
                        });
                }
            }

            return newAssets;
        }


        /// <summary>
        /// Reduce market list of each asset in dictionary
        /// </summary>
        /// <param name="assetMarketMap"></param>
        private static void ReduceToCommon(Dictionary<Asset, List<string>> assetMarketMap)
        {
            var assetMarketsWereReduced = false;
            while (true)
            {
                var longList = assetMarketMap.FirstOrDefault(i => i.Value.Count == assetMarketMap.Max(j => j.Value.Count));
                var longAsset = longList.Key;
                var longMarkets = longList.Value;
                var shortList = assetMarketMap.FirstOrDefault(i => i.Value.Count == assetMarketMap.Min(j => j.Value.Count));

                if (longList.Value.Count() == shortList.Value.Count())
                    return;

                for (var i = longMarkets.Count - 1; i >= 0; i--)
                {
                    var secondAsset = GetSecondAsset(longAsset.Name, longMarkets[i]);

                    if (shortList.Value.Any(item => item.StartsWith(secondAsset) || item.EndsWith(secondAsset)))
                        continue;

                    longMarkets.RemoveAt(i);
                    assetMarketsWereReduced = true;
                }

                if (!assetMarketsWereReduced)
                    return;

                assetMarketsWereReduced = false;
            }
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

        public DataCollectorService(IAssetRepository assetRepository, IConnector binanceConnector)
        {
            _assetRepository = assetRepository;
            _binanceConnector = binanceConnector;
        }

        private readonly IAssetRepository _assetRepository;
        private readonly IConnector _binanceConnector;
    }
}
