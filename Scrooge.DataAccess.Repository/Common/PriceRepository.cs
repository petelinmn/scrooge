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
    public class PriceRepository : BaseRepository, IPriceRepository
    {
        public List<Price> GetPrices()
        {
            var result = Connection.Query<Price>($@"
                    select m.Id, concat(a1.Name, a2.Name) as Name, m.AssetId1, m.AssetId2 
                        from public.markets m 
                            join public.assets a1 on a1.Id = m.AssetId1
                            join public.assets a2 on a2.Id = m.AssetId2
                ",
                new { },
                transaction: Transaction);
            return result.ToList();
        }
        public List<Price> GetLastPrices()
        {
            var result = Connection.Query<Price>($@"
                    SELECT id, marketid, date, value
	                    FROM public.prices Where id in
                             (SELECT Max(id) FROM public.prices
                             group by marketid)
                ",
                new { },
                transaction: Transaction);
            return result.ToList();
        }

        public void Save(List<Price> prices)
        {
            foreach (var price in prices)
            {
                Connection.Execute($@"
                    insert into Prices values (nextval('prices_id_seq'),@{nameof(price.MarketId)},@{nameof(price.Date)},@{nameof(price.Value)})",
                    new { price.MarketId, price.Date, price.Value }, Transaction);
            }
        }

        public PriceRepository(IAssetRepository assetRepository, IConnectionProvider connectionProvider) : base(connectionProvider)
        {
            _assetRepository = assetRepository;
        }
        IAssetRepository _assetRepository { get; set; }
    }
}
