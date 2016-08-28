using Cloud.Common.Configuration;
using Cloud.Common.Core;
using Cloud.Common.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.WebSockets;
using System.Threading;
using Cloud.Common.Contracts;
using System.Threading.Tasks;
using Cloud.Common.Extensions;
using Cloud.Common;

namespace Cloud.Server.Core
{
    public class MasterMessageDispatcher : MessageDispatcher, IMasterMessageDispatcher
    {
        public MasterMessageDispatcher(
            IOptions<ServerSettings> settings,
            IClientMessageFactory clientMessageFactory,
            WebSocket webSocket,
            Guid masterClientId,
            ClientType clientType)
            :base(settings, clientMessageFactory, webSocket, masterClientId, clientType)
        {
        }

        public async Task SendConnectAsJsonAsync(Guid clientId, ClientType clientType)
        {
            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var buffer = new ArraySegment<byte>(_clientMessageFactory.CreateJsonMessage<object>(
                Resolvers.NotifyConnected, new { ClientId = clientId, ClientType = clientType }));
            await _webSocket.SendAsync(buffer, type, true, token);
        }

        public async Task SendMessageAsync(OperationResultContext context)
        {
            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var message = context.CreateJson().ToUtf8Bytes();
            var buffer = new ArraySegment<byte>(message);
            await _webSocket.SendAsync(buffer, type, true, token);
        }
    }
}
