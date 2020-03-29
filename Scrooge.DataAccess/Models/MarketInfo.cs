using System;
using System.Collections.Generic;
using System.Text;

namespace Scrooge.DataAccess.Models
{
    public class MarketInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Asset Asset1 { get; set; }
        public Asset Asset2 { get; set; }
        public bool IsActive { get; set; }
    }
}
