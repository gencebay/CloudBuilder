using Cloud.Common.Contracts;
using Cloud.Common.Core;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Cloud.Common.Interfaces
{
    public interface IMessageDispatcher
    {
        ClientType ClientType { get; }
        WebSocket WebSocet { get; }
        void SendMessage(object state);
        Task SendConnectAsJsonAsync();
    }
}