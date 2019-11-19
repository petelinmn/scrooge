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
    public class AssetRepository : BaseRepository, IAssetRepository
    {

        public List<Asset> GetAssets()
        {
            var result = Connection.Query<Asset>($@"
                    select Id, Name, IsMain, IsStable from public.Assets
                ",
                new {},
                transaction: Transaction);
            return result.ToList();
        }


        public List<Market> GetMarkets()
        {
            var result = Connection.Query<Market>($@"
                    select m.Id, concat(a1.Name, a2.Name) as Name, m.AssetId1, m.AssetId2 
                        from public.markets m 
                            join public.assets a1 on a1.Id = m.AssetId1
                            join public.assets a2 on a2.Id = m.AssetId2
                ",
                new { },
                transaction: Transaction);
            return result.ToList();
        }
        
        public void Save(List<Asset> assets)
        {
            foreach (var asset in assets)
            {
                Connection.Execute($@"
                    insert into Assets values (nextval('assets_id_seq'),@{nameof(asset.Name)},@{nameof(asset.IsMain)},@{nameof(asset.IsStable)})",
                    new { asset.Name, asset.IsMain, asset.IsStable }, Transaction);
            }
        }

        public void Save(List<Market> markets)
        {
            foreach (var market in markets)
            {
                Connection.Execute($@"
                    insert into Markets values (nextval('markets_id_seq'),@{nameof(market.AssetId1)},@{nameof(market.AssetId2)})",
                    new { market.AssetId1, market.AssetId2 }, Transaction);
            }
        }

        public AssetRepository(IConnectionProvider connectionProvider) : base(connectionProvider) { }
    }
}
