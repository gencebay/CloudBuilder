using Cloud.Common.Contracts;
using Cloud.Common.Core;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Cloud.Common.Interfaces
{
    public interface IMessageDispatcher
    {
        Guid ClientId { get; }
        ClientType ClientType { get; }
        WebSocket WebSocet { get; }
        Task SendMessageAsync();
    }
}