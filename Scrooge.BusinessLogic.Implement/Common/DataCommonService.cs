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
            var tickerResult = await _binanceConnector.TickerAllPrices();
            var allMarket = _marketRepository.GetMarkets();
            var generatedPrice = GetNewPrice(tickerResult, allMarket);
            return false;
        }

        private static List<Price> GetNewPrice (IList<PriceInfo> tickerResult, IEnumerable<Market> allMarket)
        {
            List<Price> newPrice = new List<Price>();
            // Пробовал сделать по крутому. не вышло. Сделал форичами за 3 минуты.
            //var newPrice_2 = tickerResult.Where(i => allMarket.Any(a => a.Name == i.Symbol)).Select(s => new Price() { MarketId = Convert.ToInt32(allMarket.Where(m => m.Name == s.Symbol).Select(s_2 =>s_2.Id)), Value = s.Price, Date = DateTime.Now }).ToList();
            foreach (var a in allMarket)
            {
                foreach (var t in tickerResult)
                {
                    if (a.Name == t.Symbol)
                    {
                        newPrice.Add(new Price() { MarketId = a.Id, Value = t.Price, Date = DateTime.Now });
                    }
                }
            }
            return newPrice;
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
        private static List<Market> DefineNewMarkets (IEnumerable<Asset> assets, IEnumerable<Market> markets, IEnumerable<string> allSymbols)
        {
           List<Market> newMarkets = new List<Market>();
           foreach (var a in assets)
                foreach (var a2 in assets)
                    if (allSymbols.Any(i => i == a.Name + a2.Name) && !markets.Any(i => i.AssetId1 == a2.Id && i.AssetId2 == a.Id))
                        newMarkets.Add(new Market() { AssetId1 = a2.Id, AssetId2 = a.Id });

            return newMarkets;
        }

        public DataCommonService(IAssetRepository assetRepository, IMarketRepository marketRepository, IConnector binanceConnector)
        {
            _assetRepository = assetRepository;
            _marketRepository = marketRepository;
            _binanceConnector = binanceConnector;
        }

        private readonly IAssetRepository _assetRepository;
        private readonly IConnector _binanceConnector;
        private readonly IMarketRepository _marketRepository;
    }
}
