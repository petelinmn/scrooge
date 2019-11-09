using System;
using System.Collections.Generic;
using System.Text;

namespace Scrooge.DataAccess
{
    public interface IConnectionStringProvider
    {
        string GetDefaultConnectionString();
    }
}
