using Cloud.Common.Configuration;
using Cloud.Common.Contracts;
using Cloud.Common.Core;
using Cloud.Common.Interfaces;
using Cloud.Server.Core;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Cloud.Server.Middleware
{
    public class SocketRequestMiddleware
    {
        private readonly RequestDelegate next;

        public SocketRequestMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, 
            ILoggerFactory loggerFactory,
            IOptions<ServerSettings> settings,
            IClientMessageFactory messageFactory,
            ConcurrentBag<IMessageDispatcher> dispatcherBag)
        {
            var logger = loggerFactory.CreateLogger(nameof(SocketRequestMiddleware));

            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                    var clientType = ClientType.Console;

                    if (httpContext.Request.Headers["User-Agent"].Count > 0)
                        clientType = ClientType.Browser;

                    if (httpContext.Request.Query["owner"].Count > 0)
                        clientType = httpContext.Request.Query["owner"].ToString() == "Master" ? ClientType.Master : ClientType.Console;
                    
                    var messageDispatcher = new MessageDispatcher(settings, messageFactory, webSocket, clientType);
                    dispatcherBag.Add(messageDispatcher);
                    logger.LogInformation("WebSocket Initiated");

                    if (clientType != ClientType.Master && clientType != ClientType.Unset)
                        await NotifyToMaster(dispatcherBag.Where(x => x.ClientType == ClientType.Master).FirstOrDefault());
                }
                else
                {
                    // noop
                }
            }
            else
            {
                // Nothing to do here, pass downstream.  
                await next(httpContext);
            }
        }

        public async Task NotifyToMaster(IMessageDispatcher masterDispatcher)
        {
            await masterDispatcher.SendConnectAsJsonAsync();
        }
    }
}
