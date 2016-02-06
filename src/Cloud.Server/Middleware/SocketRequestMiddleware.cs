using Cloud.Common.Configuration;
using Cloud.Common.Contracts;
using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using Cloud.Server.Core;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
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
                    var messageDispatcher = new MessageDispatcher(settings, messageFactory, webSocket);
                    dispatcherBag.Add(messageDispatcher);
                    logger.LogInformation("WebSocket Initiated");
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
    }
}
