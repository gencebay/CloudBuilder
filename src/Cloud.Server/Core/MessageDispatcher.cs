using Cloud.Common.Configuration;
using Cloud.Common.Core;
using Cloud.Common.Interfaces;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Net.WebSockets;
using System.Threading;
using Cloud.Common.Contracts;
using System.Threading.Tasks;

namespace Cloud.Server.Core
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IOptions<ServerSettings> _settings;
        private readonly IClientMessageFactory _clientMessageFactory;
        private readonly Timer _timer;
        private readonly WebSocket _webSocket;
        private readonly ClientType _clientType;
        private readonly DateTime _createdDate;

        public WebSocket WebSocet
        {
            get
            {
                return _webSocket;
            }
        }

        public ClientType ClientType
        {
            get
            {
                return _clientType;
            }
        }

        public MessageDispatcher(
            IOptions<ServerSettings> settings,
            IClientMessageFactory clientMessageFactory, 
            WebSocket webSocket,
            ClientType clientType)
        {
            _settings = settings;
            _clientMessageFactory = clientMessageFactory;
            _webSocket = webSocket;
            _timer = new Timer(new TimerCallback(SendMessage), new { init = true }, 0, settings.Value.DispatchInterval);
            _clientType = clientType;
            _createdDate = DateTime.Now;
        }

        public void SendMessage(object state)
        {
            var randomMessageIndex = Environments.Random.Next(RandomMessageGenerator.Messages.Count);
            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var message = RandomMessageGenerator.Messages[randomMessageIndex];
            ArraySegment<byte> buffer = new ArraySegment<byte>();

            if (_clientType == ClientType.Browser)
                buffer = new ArraySegment<byte>(_clientMessageFactory.CreateJsonMessage(message));
            else
                buffer = new ArraySegment<byte>(_clientMessageFactory.CreateXmlMessage(message));

            _webSocket.SendAsync(buffer, type, true, token);
        }

        public async Task SendConnectAsJsonAsync()
        {
            var command = new CommandDefinitions
            {
                ClientType = _clientType,
                Commands = new[] { CommandType.Connect },
                Owner = "Server1",
                CreatedDate = _createdDate
            };

            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var buffer = new ArraySegment<byte>(_clientMessageFactory.CreateJsonMessage(command));
            await _webSocket.SendAsync(buffer, type, true, token);
        }
    }
}
