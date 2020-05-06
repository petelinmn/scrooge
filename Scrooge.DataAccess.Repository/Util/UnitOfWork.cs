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

        IDbConnection _connection = null;
        IDbTransaction _transaction = null;

        public UnitOfWork(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void Begin()
        {
            if (_connection == null)
                _connection = _connectionProvider.GetConnection();

            _connectionProvider.SetConnection(_connection);

            if (_transaction == null)
                _transaction = _connectionProvider.OpenTransaction();
            
            _connectionProvider.SetTransaction(_transaction);
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
                _transaction = null;
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
