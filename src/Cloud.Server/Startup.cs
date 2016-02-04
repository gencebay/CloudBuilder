using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace Cloud.Server
{
    public class Startup
    {
        private readonly ConcurrentBag<WebSocket> _socketsBag;
        private readonly Timer _timer;

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _socketsBag = new ConcurrentBag<WebSocket>();
            _timer = new Timer(new TimerCallback(TimerCallback), new { init = true }, 0, 5000);
        }

        private void TimerCallback(object state)
        {
            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var data = Encoding.UTF8.GetBytes($"Push from server: {DateTime.Now.ToString()}");
            var buffer = new ArraySegment<byte>(data);

            if (_socketsBag.Count > 0)
            {
                var webSocket = _socketsBag.FirstOrDefault(x => x.State == WebSocketState.Open);
                webSocket.SendAsync(buffer, type, true, token);
            }
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseWebSockets();

            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    if (webSocket != null && webSocket.State == WebSocketState.Open)
                    {
                        // TODO: Handle the socket here.
                        _socketsBag.Add(webSocket);
                    }
                    else
                    {
                        _socketsBag.TryTake(out webSocket);
                    }
                }
                else
                {
                    // Nothing to do here, pass downstream.  
                    await next();
                }
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
