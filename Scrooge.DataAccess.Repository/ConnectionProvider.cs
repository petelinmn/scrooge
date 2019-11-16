using System;
using System.Data;
using Npgsql;

namespace Scrooge.DataAccess.Repository
{
    public class ConnectionProvider : IConnectionProvider
    {
        private IDbTransaction _transaction;
        private IDbConnection _connection;
        private bool _disposed = false;

        private readonly string _connectionString;

        public ConnectionProvider(IConnectionStringProvider connectionStringProvider)
        {
            _connectionString = connectionStringProvider.GetDefaultConnectionString();
        }

        public IDbConnection GetConnection()
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
            return _connection;
        }
        public IDbTransaction OpenTransaction()
        {
            _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
            return _transaction;
        }
        
        public IDbConnection CurrentConnection
        {
            get
            {
                if (_connection == null)
                {
                    GetConnection();
                }
                return _connection;
            }
        }

        public IDbTransaction CurrentTransaction
        {
            get
            {
                if (_transaction == null)
                {
                    OpenTransaction();
                }
                return _transaction;
            }
        }

        public IDbTransaction SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
            return _transaction;
        }

        public IDbConnection SetConnection(IDbConnection connection)
        {
            _connection = connection;
            return _connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~ConnectionProvider()
        {
            Dispose(false);
        }
    }
}
