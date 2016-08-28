using Cloud.Common;
using Cloud.Common.Configuration;
using Cloud.Common.Core;
using Cloud.Common.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Server.Core
{
    public class MessageDispatcher : IMessageDispatcher
    {
        protected readonly IOptions<ServerSettings> _settings;
        protected readonly IClientMessageFactory _clientMessageFactory;
        protected readonly WebSocket _webSocket;
        protected readonly ClientType _clientType;
        protected readonly DateTime _createdDate;
        protected readonly Guid _clientId;

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

        public Guid ClientId
        {
            get
            {
                return _clientId;
            }
        }

        public MessageDispatcher(
            IOptions<ServerSettings> settings,
            IClientMessageFactory clientMessageFactory, 
            WebSocket webSocket,
            Guid clientId,
            ClientType clientType)
        {
            _settings = settings;
            _clientMessageFactory = clientMessageFactory;
            _webSocket = webSocket;
            _clientId = clientId;
            _clientType = clientType;
            _createdDate = DateTime.Now;
        }

        public async Task SendMessageAsync()
        {
            var randomMessageIndex = Globals.Random.Next(RandomMessageGenerator.Messages.Count);
            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var message = RandomMessageGenerator.Messages[randomMessageIndex];
            ArraySegment<byte> buffer = new ArraySegment<byte>();

            if (_clientType == ClientType.Browser)
                buffer = new ArraySegment<byte>(_clientMessageFactory.CreateJsonMessage(message));
            else
                buffer = new ArraySegment<byte>(_clientMessageFactory.CreateXmlMessage(message));

            await _webSocket.SendAsync(buffer, type, true, token);
        }
    }
}
