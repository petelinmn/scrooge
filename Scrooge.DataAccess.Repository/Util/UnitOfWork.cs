using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Scrooge.DataAccess.Util;

namespace Scrooge.DataAccess.Repository.Util
{
    public class UnitOfWork : IUnitOfWork
    {
        protected IConnectionProvider _connectionProvider;
        protected bool _disposed;

        public UnitOfWork(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void Begin(IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                _connectionProvider.GetConnection();
                _connectionProvider.OpenTransaction();
            }
            else
            {
                _connectionProvider.SetConnection(connection);
                if (transaction == null)
                {
                    _connectionProvider.OpenTransaction();
                }
                else
                {
                    _connectionProvider.SetTransaction(transaction);
                }
            }
        }

        public void Commit()
        {
            try
            {
                _connectionProvider.CurrentTransaction.Commit();
            }
            catch
            {
                _connectionProvider.CurrentTransaction.Rollback();
                throw;
            }
            finally
            {
                _connectionProvider.CurrentTransaction.Dispose();
            }
        }

        public IDbConnection CurrentConnection
        {
            get { return _connectionProvider?.CurrentConnection; }
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connectionProvider != null)
                    {
                        _connectionProvider.Dispose();
                        _connectionProvider = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
