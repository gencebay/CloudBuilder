using Cloud.Common;
using Cloud.Common.Configuration;
using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using Cloud.Server.Core;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
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
            IServiceCollection services,
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
                    // ClientId, Type and IsMaster
                    Tuple<Guid, ClientType, bool> dispatchObjs = httpContext.DispatcherHelper();
                    IMessageDispatcher dispatcher;
                    if (dispatchObjs.Item3)
                    {
                        dispatcher = new MasterMessageDispatcher(settings,
                            messageFactory,
                            webSocket,
                            dispatchObjs.Item1,
                            dispatchObjs.Item2);

                        // DI Container Registration for controller simple access to Master Dispatcher
                        services.AddInstance(typeof(IMasterMessageDispatcher), dispatcher);
                    }
                    else
                    {
                        dispatcher = new MessageDispatcher(settings,
                             messageFactory,
                             webSocket,
                             dispatchObjs.Item1,
                             dispatchObjs.Item2);
                    }

                    dispatcherBag.Add(dispatcher);
                    logger.LogInformation("WebSocket Initiated");

                    // Ignore notification for self connection of Master App
                    if (!dispatchObjs.Item3)
                        await NotifyToMaster(dispatcherBag, dispatchObjs.Item1, dispatchObjs.Item2);
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

        public async Task NotifyToMaster(ConcurrentBag<IMessageDispatcher> dispatcher, Guid clientId, ClientType clientType)
        {
            var masterDispatcher = (IMasterMessageDispatcher)dispatcher.Where(x => x.ClientType == ClientType.Master).FirstOrDefault();
            if (masterDispatcher != null) { }
                await masterDispatcher.SendConnectAsJsonAsync(clientId, clientType);
        }
    }
}
