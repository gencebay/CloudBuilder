using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Cloud.Common.Interfaces
{
    public interface IMessageDispatcher
    {
        void SendMessage(object state);
    }
}