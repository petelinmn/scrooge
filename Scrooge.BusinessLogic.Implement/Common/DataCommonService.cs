using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scrooge.Exchange.Connectors;
using Scrooge.DataAccess.Common;
using Scrooge.DataAccess.Models;
using Scrooge.BusinessLogic.Common;

namespace Scrooge.BusinessLogic.Implement.Common
{
    public class DataCommonService : IDataCommonService
    {
        public void Collect(DateTime requestDate)
        {
            var allMarkets = MarketRepository.GetMarkets(true);
            var allPrices = GetAllPrices(allMarkets, requestDate).ToList();
            var newPrices = GetNewPrices(allPrices).ToList();

            PriceRepository.Save(newPrices);
        }

        private IEnumerable<Price> GetAllPrices(IEnumerable<Market> allMarkets,
            DateTime requestDate)
        {
            var tickerResult = BinanceConnector.TickerAllPrices().Result;
            return from market in allMarkets
                from ticker in tickerResult
                where market.Name == ticker.Symbol
                select new Price() {MarketId = market.Id, Value = ticker.Price, Date = requestDate};
        }

        private IEnumerable<Price> GetNewPrices(IEnumerable<Price> allPrices)
        {
            var lastPrices = PriceRepository.GetLastPrices().ToList();
            return allPrices
                .Where(i => (lastPrices
                                 .Any(l => l.MarketId == i.MarketId && l.Value != i.Value)
                             || (lastPrices.All(l => l.MarketId != i.MarketId)))).ToList();
        }

        /// <summary>
        /// First initialize collections of assets and markets
        /// </summary>
        /// <returns></returns>
        public async Task MarketCollectionInitialize()
        {
            var tickerResult = await BinanceConnector.TickerAllPrices();
            var allSymbols = tickerResult.Select(i => i.Symbol).ToList();

            var newAssets = DefineNewAssets(allSymbols);
            AssetRepository.Save(newAssets);
            
            var newMarkets = DefineNewMarkets(allSymbols);
            MarketRepository.Save(newMarkets);
        }

        /// <summary> 
        /// Define new assets
        /// </summary>
        /// <param name="allSymbols"></param>
        /// <returns></returns>
        private List<Asset> DefineNewAssets(IReadOnlyCollection<string> allSymbols)
        {
            var allAssets = AssetRepository.GetAssets();
            var primaryAssets = allAssets
                .Where(i => i.IsMain == true)
                .ToDictionary(i => i, i => allSymbols
                    .Where(t => t.EndsWith(i.Name))
                        .Select(s => s).ToList());

            return primaryAssets
                .ToDictionary(i => i.Key, i => i.Value
                     .Where(v => primaryAssets
                         .All(i2 => i2.Value
                             .Any(v2 => v2.Replace(i2.Key.Name, "") == v.Replace(i.Key.Name, ""))))
                     .Where(v => allAssets.All(a => a.Name == v.Replace(i.Key.Name, "")))
                     .Select(s => new Asset() { Name = s.Replace(i.Key.Name, ""), IsMain = false, IsStable = false }).ToList())
                .FirstOrDefault().Value;
        }

        /// <summary>
        /// Define new markets
        /// </summary>
        /// <param name="allSymbols"></param>
        /// <returns></returns>
        private List<Market> DefineNewMarkets (IReadOnlyCollection<string> allSymbols)
        {
           var allMarkets = MarketRepository.GetMarkets().ToList();
           var allAssets = AssetRepository.GetAssets();
           var newMarkets = new List<Market>();
           foreach (var a in allAssets)
                foreach (var a2 in allAssets.Where(i => i.IsMain || (!i.IsMain && !a.IsMain)))
                    if (allSymbols.Any(i => i == a.Name + a2.Name) && !allMarkets.Any(i => i.AssetId1 == a.Id && i.AssetId2 == a2.Id)
                        && !allMarkets.Any(i => i.AssetId1 == a.Id && i.AssetId2 == a2.Id) 
                        && !newMarkets.Any(i => i.AssetId1 == a.Id && i.AssetId2 == a2.Id))
                        newMarkets.Add(new Market() { AssetId1 = a.Id, AssetId2 = a2.Id, IsActive = true });

           return newMarkets;
        }

        public DataCommonService(IAssetRepository assetRepository, IMarketRepository marketRepository, IPriceRepository priceRepository, IConnector binanceConnector)
        {
            AssetRepository = assetRepository;
            MarketRepository = marketRepository;
            PriceRepository = priceRepository;
            BinanceConnector = binanceConnector;
        }

        private IAssetRepository AssetRepository { get; }
        private IConnector BinanceConnector { get; }
        private IMarketRepository MarketRepository { get; }
        private IPriceRepository PriceRepository { get; }
    }
}
