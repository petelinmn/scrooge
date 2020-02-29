using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrooge.BusinessLogic.Common
{
    public interface IDataCommonService
    {
        /// <summary>
        /// First initialize collections of assets and markets
        /// </summary>
        /// <returns></returns>
        Task<bool> MarketCollectionInitialize();
    }
}
