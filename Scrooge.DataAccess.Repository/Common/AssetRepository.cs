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

        public void Save(List<Asset> assets)
        {
            foreach (var asset in assets)
            {
                Connection.Execute($@"
                    insert into Assets values (nextval('assets_id_seq'),@{nameof(asset.Name)},@{nameof(asset.IsMain)},@{nameof(asset.IsStable)})",
                    new { asset.Name, asset.IsMain, asset.IsStable }, Transaction);
            }
        }

        public AssetRepository(IConnectionProvider connectionProvider) : base(connectionProvider) { }
    }
}
