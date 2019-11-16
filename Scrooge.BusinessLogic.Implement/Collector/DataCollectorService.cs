using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Scrooge.BusinessLogic.Collector;
using Scrooge.Exchange.Connectors;
using Scrooge.Exchange.Connectors.BinanceConnector;
using Scrooge.DataAccess.Common;
using Scrooge.DataAccess.Models;

namespace Scrooge.BusinessLogic.Implement.Collector
{
    public class DataCollectorService : IDataCollectorService
    {
        public void MarketDefinition()
        {
            IConnector bc = new BinanceConnector();

            var assets = _assetRepository.GetAssets().ToList();
            var primaryAssets = assets.Where(i => i.IsMain);

            var tickerResult = bc.TickerAllPrices().Result;
            
            var assetMarketDict = new Dictionary<Asset, List<string>>();
            foreach (var asset in primaryAssets)
            {
                var assetMarkets = tickerResult.Where(i => i.Symbol.EndsWith(asset.Name)).Select(i => i.Symbol).OrderBy(i=>i).ToList();
                assetMarketDict.Add(asset, assetMarkets);
            }

            bool assetMarketsWereReduced = true;
            while (true)
            {
                var longList = assetMarketDict.FirstOrDefault(i => i.Value.Count == assetMarketDict.Max(j => j.Value.Count));
                var longAsset = longList.Key;
                var longMarkets = longList.Value;
                var shortList = assetMarketDict.FirstOrDefault(i => i.Value.Count == assetMarketDict.Min(j => j.Value.Count));

                if (longList.Value.Count() == shortList.Value.Count())
                    continue;

                for (var i = longMarkets.Count - 1; i >= 0; i--)
                {
                    var market = longMarkets[i];
                    var secondAsset = GetSecondAsset(longAsset.Name, market);

                    if (shortList.Value.Any(item => item.StartsWith(secondAsset) || item.EndsWith(secondAsset)))
                        continue;
                    
                    longMarkets.RemoveAt(i);
                    assetMarketsWereReduced = true;
                }

                if (shortList.Value.Count == 0)
                {
                    throw new Exception("It's requires to reduce list of main assets!");
                }

                if (assetMarketsWereReduced)
                    assetMarketsWereReduced = false;
                else
                    break;
            }
            
            var newAssets = new List<Asset>();
            foreach (var assetDict in assetMarketDict)
            {
                var markets = assetDict.Value;
                var asset = assetDict.Key;
                foreach (var market in markets)
                {
                    var secondAsset = GetSecondAsset(asset.Name, market);
                    if(!newAssets.Any(item => item.Name == secondAsset) && !assets.Any(item => item.Name == secondAsset))
                        newAssets.Add(new Asset()
                        {
                            Name = secondAsset,
                            IsMain = false,
                            IsStable = false
                        });
                }    
            }

            _assetRepository.Save(newAssets);

            var allMarkets = _assetRepository.GetMarkets();

            var newMarkets = new List<Market>();
            assets.AddRange(newAssets);
            foreach (var ticker in tickerResult)
            {
                if(allMarkets.Any(i => i.Name == ticker.Symbol))
                    continue;

                foreach (var asset in assets)
                {
                    if (ticker.Symbol.StartsWith(asset.Name))
                    {
                        var secondAssetName = ticker.Symbol.Substring(asset.Name.Length,
                            ticker.Symbol.Length - asset.Name.Length);
                        var secondAsset = assets.Where(a => a.Name == secondAssetName).FirstOrDefault();
                        if (assets.Any(i => i.Name == secondAssetName) && !newMarkets.Any(i => i.Name == ticker.Symbol))
                        {
                            newMarkets.Add(new Market() { Name = ticker.Symbol, AssetId1 = asset.Id, AssetId2 = secondAsset.Id });
                        }
                    }
                }
            }

            _assetRepository.Save(newMarkets);
        }

        private string GetSecondAsset(string Asset, string Market)
        {
            if (Market.StartsWith(Asset))
            {
                return Market.Substring(Asset.Length, Market.Length - Asset.Length);
            }
            else
            {
                return Market.Substring(0, Market.Length - Asset.Length);
            }
        }

        public DataCollectorService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        private readonly IAssetRepository _assetRepository;
    }
}
