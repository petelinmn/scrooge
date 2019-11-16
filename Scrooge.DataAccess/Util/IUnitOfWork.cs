using System;
using System.Data;

namespace Scrooge.DataAccess.Util
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// open Connection and Transaction
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        void Begin(IDbConnection connection = null, IDbTransaction transaction = null);

        void Commit();

        IDbConnection CurrentConnection { get; }
    }
}