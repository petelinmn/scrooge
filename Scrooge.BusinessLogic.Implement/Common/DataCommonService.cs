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
        public async Task<bool> Collect()
        {
            var allMarket = _marketRepository.GetMarkets(true);
            var requestDate = DateTime.Now;
            var tickerResult = await _binanceConnector.TickerAllPrices();
            var generatedPrice = GeneratedPrice(tickerResult, allMarket, requestDate);
            var lastPrices = _priceRepository.GetLastPrices();
            var newPrices = GetNewPrice(generatedPrice, lastPrices);
            _priceRepository.Save(newPrices);
            return false;
        }

        private static List<Price> GeneratedPrice(IList<PriceInfo> tickerResult, IEnumerable<Market> allMarket, DateTime requestDate)
        {
            List<Price> result = new List<Price>();
            foreach (var a in allMarket)
                foreach (var t in tickerResult)
                    if (a.Name == t.Symbol)
                        result.Add(new Price() { MarketId = a.Id, Value = t.Price, Date = requestDate });
            return result;
        }

        private static List<Price> GetNewPrice(List<Price> generatedPrice, List<Price> lastPrices)
        {

            return generatedPrice
                .Where(i => (lastPrices
                .Any(l => l.MarketId == i.MarketId && l.Value != i.Value)
                ||(lastPrices.All(l=>l.MarketId!=i.MarketId)))).ToList();
        }
        /// <summary>
        /// First initialize collections of assets and markets
        /// </summary>
        /// <returns></returns>
        public async Task<bool> MarketCollectionInitialize()
        {
            var tickerResult = await _binanceConnector.TickerAllPrices();
            var allSymbols = tickerResult.Select(i => i.Symbol);

            var allAssets = _assetRepository.GetAssets();
            var newAssets = DefineNewAssets(allAssets, allSymbols);
            _assetRepository.Save(newAssets);

            allAssets = _assetRepository.GetAssets();
            var allMarkets = _marketRepository.GetMarkets();
            var newMarkets = DefineNewMarkets(allAssets, allMarkets, allSymbols);
            _marketRepository.Save(newMarkets);

            return false;
        }

        /// <summary> 
        /// Define new assets
        /// </summary>
        /// <param name="assets">Existing assets</param>
        /// <param name="tickerResult">set of markets from exchange</param>
        /// <returns></returns>
        private static List<Asset> DefineNewAssets(IEnumerable<Asset> assets, IEnumerable<string> allSymbols)
        {
            var primaryAssets = assets
                .Where(i => i.IsMain == true)
                .ToDictionary(i => i, i => allSymbols
                    .Where(t => t.EndsWith(i.Name))
                        .Select(s => s).ToList());

            return primaryAssets
                .ToDictionary(i => i.Key, i => i.Value
                     .Where(v => primaryAssets
                         .All(i2 => i2.Value
                             .Any(v2 => v2.Replace(i2.Key.Name, "") == v.Replace(i.Key.Name, ""))))
                     .Where(v => !assets.Any(a => a.Name == v.Replace(i.Key.Name, "")))
                     .Select(s => new Asset() { Name = s.Replace(i.Key.Name, ""), IsMain = false, IsStable = false }).ToList())
                .FirstOrDefault().Value;
        }

        /// <summary>
        /// Define new markets
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="allSymbols"></param>
        /// <returns></returns>
        private static List<Market> DefineNewMarkets (IEnumerable<Asset> assets, IEnumerable<Market> allMarkets, IEnumerable<string> allSymbols)
        {
           List<Market> newMarkets = new List<Market>();
           foreach (var a in assets)
                foreach (var a2 in assets.Where(i => i.IsMain || (!i.IsMain && !a.IsMain)))
                    if (allSymbols.Any(i => i == a.Name + a2.Name) && !allMarkets.Any(i => i.AssetId1 == a.Id && i.AssetId2 == a2.Id)
                        && !allMarkets.Any(i => i.AssetId1 == a.Id && i.AssetId2 == a2.Id) 
                        && !newMarkets.Any(i => i.AssetId1 == a.Id && i.AssetId2 == a2.Id))
                        newMarkets.Add(new Market() { AssetId1 = a.Id, AssetId2 = a2.Id, IsActive = true });

            return newMarkets;
        }

        public DataCommonService(IAssetRepository assetRepository, IMarketRepository marketRepository, IPriceRepository priceRepository, IConnector binanceConnector)
        {
            _assetRepository = assetRepository;
            _marketRepository = marketRepository;
            _priceRepository = priceRepository;
            _binanceConnector = binanceConnector;
        }

        private readonly IAssetRepository _assetRepository;
        private readonly IConnector _binanceConnector;
        private readonly IMarketRepository _marketRepository;
        private readonly IPriceRepository _priceRepository;

    }
}
