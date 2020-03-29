using System;
using System.Collections.Generic;
using System.Text;

namespace Scrooge.DataAccess.Models
{
    public class Market
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AssetId1 { get; set; }
        public int AssetId2 { get; set; }
        public bool IsActive { get; set; }
    }

    public class Price
    {
        public int Id { get; set; }
        public int MarketId { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
