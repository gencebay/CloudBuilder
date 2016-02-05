using Cloud.Common.Interfaces;
using Cloud.Server.Core;
using Cloud.Server.Interfaces;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
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
            IClientMessageFactory messageFactory,
            ConcurrentBag<IMessageDispatcher> dispatcherBag)
        {
            var logger = loggerFactory.CreateLogger(nameof(SocketRequestMiddleware));

            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                    // TODO: Handle the socket here.
                    var messageDispatcher = new MessageDispatcher(messageFactory, webSocket);
                    dispatcherBag.Add(messageDispatcher);
                    logger.LogInformation("WebSocket Initiated");
                }
                else
                {
                    logger.LogWarning("WebSocket closed!");
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
