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

            var newAssets = GetNewAsset(allAssets, tickerResult);

            _assetRepository.Save(newAssets);

            var allMarkets = _assetRepository.GetMarkets();

            var newMarket = GetNewMarkets(allAssets, tickerResult);

            _assetRepository.Save(newMarket);



            return false;
        }

        /// <summary> 
        /// Get mapping dictionary for asset and collection its markets
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="tickerResult"></param>
        /// <returns></returns>
        private static List<Asset> GetNewAsset(IEnumerable<Asset> assets, IList<PriceInfo> tickerResult)
        {
            var primaryAssets = assets.Where(i => i.IsMain == true).ToDictionary(i => i, i => tickerResult.Where(t => t.Symbol.EndsWith(i.Name)).Select(s => s.Symbol).ToList());
            Dictionary<string, int> text = new Dictionary<string, int>();
            foreach (var primAss in primaryAssets)
            {
                foreach (var primAssValue in primAss.Value)
                {
                    var str = primAssValue.Replace(primAss.Key.Name, "");

                    if (text.ContainsKey(str))
                        text[str]++;
                    else
                        text.Add(str, 1);
                }
            }
            return  text.Where(i => !assets.Any(a => a.Name == i.Key)).Where(i => i.Value == assets.Count()).Select(i => new Asset() { Name = i.Key, IsMain = false, IsStable = false }).ToList();
        }

        private static List<Market> GetNewMarkets (IEnumerable<Asset> assets, IList<PriceInfo> tickerResult)
        {
            List<Market> newMarkets = new List<Market>();
           foreach (var a in assets)
            {
                foreach (var a_2 in assets)
                {
                    if (tickerResult.Any(i => i.Symbol == a_2.Name + a.Name))
                    {
                        newMarkets.Add(new Market() { AssetId1 = a.Id, AssetId2 = a_2.Id });
                    }
                }               
            }
            return newMarkets.Distinct().ToList();
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
