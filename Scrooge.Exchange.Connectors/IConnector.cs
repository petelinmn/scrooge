using System.Threading.Tasks;

namespace Scrooge.Exchange.Connectors
{
    public interface IConnector
    {
        Task<bool> Ping();
    }
}
