using Cloud.Common.Configuration;
using Cloud.Common.Core;
using Cloud.Common.Interfaces;
using Cloud.Server.Interfaces;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;

namespace Cloud.Server.Core
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IOptions<ServerSettings> _settings;
        private readonly IClientMessageFactory _clientMessageFactory;
        private readonly Timer _timer;
        private readonly WebSocket _webSocket;

        public MessageDispatcher(
            IOptions<ServerSettings> settings,
            IClientMessageFactory clientMessageFactory, 
            WebSocket webSocket)
        {
            _settings = settings;
            _clientMessageFactory = clientMessageFactory;
            _webSocket = webSocket;
            _timer = new Timer(new TimerCallback(SendMessage), new { init = true }, 0, settings.Value.DispatchInterval);
        }

        public void SendMessage(object state)
        {
            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var message = RandomMessageGenerator.Messages.Take(1).FirstOrDefault();
            var buffer = new ArraySegment<byte>(_clientMessageFactory.CreateMessage(message));
            _webSocket.SendAsync(buffer, type, true, token);
        }
    }
}
