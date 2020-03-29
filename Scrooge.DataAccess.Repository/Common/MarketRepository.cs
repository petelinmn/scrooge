using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Dapper;
using Scrooge.DataAccess.Common;
using Scrooge.DataAccess.Models;

namespace Scrooge.DataAccess.Repository.Common
{
    public class MarketRepository : BaseRepository, IMarketRepository
    {
        public List<Market> GetMarkets(bool onlyActive = true)
        {
            var result = Connection.Query<Market>($@"
                    select m.Id, concat(a2.Name, a1.Name) as Name, m.AssetId1, m.AssetId2, m.isactive 
                        from public.markets m 
                            join public.assets a1 on a1.Id = m.AssetId1
                            join public.assets a2 on a2.Id = m.AssetId2
                        {(onlyActive ? "Where m.isactive = true" : "")}                        
                ",
                new { },
                transaction: Transaction);
            return result.ToList();
        }

        public List<MarketInfo> GetMarketsInfo()
        {
            var markets = GetMarkets();
            var assets = _assetRepository.GetAssets();
            var result = markets.Select(i => new MarketInfo { Id = i.Id, Asset1 = assets.First(a => a.Id == i.AssetId1), Asset2 = assets.First(a => a.Id == i.AssetId1), IsActive = i.IsActive});
            return result.ToList();
        }

        public void Save(List<Market> markets)
        {
            foreach (var market in markets)
            {
                Connection.Execute($@"
                    insert into Markets values (nextval('markets_id_seq'),@{nameof(market.AssetId1)},@{nameof(market.AssetId2)},@{nameof(market.IsActive)})",
                    new { market.AssetId1, market.AssetId2, market.IsActive }, Transaction);
            }
        }


        public MarketRepository(IAssetRepository assetRepository, IConnectionProvider connectionProvider) : base(connectionProvider) 
        {
            _assetRepository = assetRepository;
        }
        IAssetRepository _assetRepository { get; set; }
    }
}
