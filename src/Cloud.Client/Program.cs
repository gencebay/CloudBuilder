using Cloud.Common.Configuration;
using Microsoft.AspNetCore.WebSockets.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Client
{
    public class Program
    {
        private static readonly Dictionary<string, string> commandLineArgs = new Dictionary<string, string>
        {
            {"--text", "showText" },
            {"-t", "showText" },
        };

        private static IServiceProvider _serviceProvider;

        private static IConfigurationRoot Configuration { get; set; }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            services.Configure<ClientSettings>(Configuration.GetSection("AppSettings"));
            _serviceProvider = services.BuildServiceProvider();
        }

        public static void Main(string[] args)
        {
            try
            {
                ConfigureServices();

                var config = new ConfigurationBuilder()
                    .AddCommandLine(args, commandLineArgs)
                    .Build();

                Console.WriteLine("Press button to connect the server. Waiting to start...");
                Console.ReadKey();
                SimpleWork().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task SimpleWork()
        {
            WebSocketClient client = new WebSocketClient();
            WebSocket webSocket = await client.ConnectAsync(new Uri("ws://localhost:5005/"), CancellationToken.None);
            while (webSocket.State == WebSocketState.Open)
            {
                var token = CancellationToken.None;
                var buffer = new ArraySegment<byte>(new byte[4096]);

                // Below will wait for a request message.
                var received = await webSocket.ReceiveAsync(buffer, token);

                switch (received.MessageType)
                {
                    case WebSocketMessageType.Text:
                        //var request = Encoding.UTF8.GetString(buffer.Array,
                        //                                      buffer.Offset,
                        //                                      buffer.Count);

                        var pushedData = Encoding.UTF8.GetString(buffer.Array).TrimEnd(new char[] { (char)0 });
                        // Handle request here.
                        Console.WriteLine("Server pushed data: " + pushedData);
                        break;
                }
            }
        }
    }
}
