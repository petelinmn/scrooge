using System;
using System.Data;

namespace Scrooge.DataAccess.Util
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// open Connection and Transaction
        /// </summary>
        void Begin();

        void Commit();

        IDbConnection CurrentConnection { get; }
    }
}