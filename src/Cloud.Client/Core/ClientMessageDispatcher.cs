using Cloud.Common.Configuration;
using Cloud.Common.Contracts;
using Cloud.Common.Core;
using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        public ClientMessageDispatcher(ClientSettings settings, WebSocket webSocket, Guid clientId)
        {
            _clientId = clientId;
            _settings = settings;
            _webSocket = webSocket;
        }

        private CommandDefinitions GetMessage(ArraySegment<byte> buffer, CancellationToken token)
        {
            var serializedObject = Encoding.UTF8.GetString(buffer.Array).TrimEnd(new char[] { (char)0 });
            return serializedObject.FromXml<CommandDefinitions>();
        }

        public async Task Listen()
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

                    if (!string.IsNullOrEmpty(command.Recipient.AssemblyName))
                        await DoWork(command);
                    else
                    {
                        var context = new OperationResultContext
                        {
                            ClientId = ClientId,
                            Command = CommandType.Build,
                            CompletedDate = DateTime.Now,
                            State = true,
                            ResultInfo = ""
                        };
                        await PostStatus(context);
                    }
                    break;
            }
        }

        private async Task PostStatus(OperationResultContext resultContext)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            HttpMethod httpMethod = HttpMethod.Get;
            UriBuilder requestUri = new UriBuilder(_settings.ApiBaseHostAddress);
            requestUri.Path = "api/Task/Completed";
            request.Method = HttpMethod.Post;
            request.RequestUri = requestUri.Uri;
            request.Content = new StringContent(JsonConvert.SerializeObject(resultContext), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                await client.SendAsync(request, CancellationToken.None);
            }
        }

        public async Task DoWork(CommandDefinitions command)
        {
            // Long running process;
            await Task.Delay(4000);
            var tobeCompiledPath = Path.Combine(Environment.CurrentDirectory, @"..\..\ToBeCompiled");
            var projectPath = $"{tobeCompiledPath}\\{command.Recipient.AssemblyName}";

            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo("cmd")
                {
                    WorkingDirectory = projectPath,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            process.Start();
            // only if the command preceding the symbol is successful
            process.StandardInput.WriteLine("dnu restore && dnu build && dnx run");

            process.StandardInput.Flush();
            process.StandardInput.Close();

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Close();

            Console.WriteLine(output);

            var context = new OperationResultContext
            {
                ClientId = ClientId,
                Command = CommandType.Build,
                CompletedDate = DateTime.Now,
                State = true,
                ResultInfo = output
            };
            await PostStatus(context);
        }
    }
}
