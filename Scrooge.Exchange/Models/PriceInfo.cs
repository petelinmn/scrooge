
namespace Scrooge.Exchange.Connectors.Models
{
    public class PriceInfo
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Symbol}: {Price}";
        }
    }
}
