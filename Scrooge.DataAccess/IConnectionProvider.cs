﻿using System;
using System.Data;

namespace Scrooge.DataAccess
{
    public interface IConnectionProvider : IDisposable
    {
        IDbConnection GetConnection();
        IDbTransaction OpenTransaction();
        IDbConnection CurrentConnection { get; }
        IDbTransaction CurrentTransaction { get; }
        IDbTransaction SetTransaction(IDbTransaction transaction);
        IDbConnection SetConnection(IDbConnection connection);
    }
}
