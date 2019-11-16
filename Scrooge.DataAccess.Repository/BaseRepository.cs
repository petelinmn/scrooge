using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Scrooge.DataAccess.Repository
{
    public abstract class BaseRepository
    {
        protected IDbTransaction Transaction => _connectionProvider.CurrentTransaction;

        protected IDbConnection Connection => _connectionProvider.CurrentTransaction.Connection;

        public BaseRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        protected IList<T> LoadEntitiesByParts<T>(IEnumerable<int> ids, Func<IEnumerable<int>, IList<T>> loader)
        {
            List<T> result = new List<T>();
            List<int> curIds = new List<int>();
            foreach (int id in ids)
            {
                curIds.Add(id);
                if (curIds.Count == 2000)
                {
                    var part = loader(curIds);
                    result.AddRange(part);
                    curIds.Clear();
                }
            }
            if (curIds.Count > 0)
            {
                var part = loader(curIds);
                result.AddRange(part);
            }
            return result;
        }

        private readonly IConnectionProvider _connectionProvider;
    }
}
