using Cloud.Common.Configuration;
using Cloud.Common.Contracts;
using Cloud.Common.Core;
using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Client.Core
{
    public class ClientMessageDispatcher : IClientMessageDispatcher
    {
        private readonly ClientSettings _settings;
        private readonly WebSocket _webSocket;
        private readonly Guid _clientId;

        public Guid ClientId
        {
            get
            {
                return _clientId;
            }
        }

        public ClientMessageDispatcher(ClientSettings settings, WebSocket webSocket)
        {
            _clientId = Guid.NewGuid();
            _settings = settings;
            _webSocket = webSocket;
            Listen().Wait();
        }

        private CommandDefinitions GetMessage(ArraySegment<byte> buffer, CancellationToken token)
        {
            var serializedObject = Encoding.UTF8.GetString(buffer.Array).TrimEnd(new char[] { (char)0 });
            return serializedObject.FromXml<CommandDefinitions>();
        }

        private async Task Listen()
        {
            var token = CancellationToken.None;
            var buffer = new ArraySegment<byte>(new byte[_settings.MaxDataSize]);

            // Below will wait for a request message.
            var received = await _webSocket.ReceiveAsync(buffer, token);

            switch (received.MessageType)
            {
                case WebSocketMessageType.Text:
                    CommandDefinitions command = GetMessage(buffer, token);
                    Console.WriteLine(
                        "Server pushed Command(s): " 
                        + string.Join(",", command.Commands) + "\t" 
                        +"to: " + command.Recipient.AssemblyName);
                    await DoWork(command);
                    break;
            }
        }

        private void SendMessage(OperationResultContext operationResult)
        {
            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var buffer = new ArraySegment<byte>(operationResult.CreateXml().ToUtf8Bytes());
            _webSocket.SendAsync(buffer, type, true, token);
        }

        public async Task DoWork(CommandDefinitions command)
        {
            // This method should stay empty
            // Following statement will prevent a compiler warning:
            await Task.Delay(10000);
            await Task.FromResult(0);
            SendMessage(new OperationResultContext
            {
                ClientId = ClientId,
                Command = CommandType.Build,
                CompletedDate = DateTime.Now,
                State = true,
                ResultInfo = "Sample result Info"
            });
        }
    }
}
