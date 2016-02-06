using Cloud.Client.Core;
using Cloud.Common.Configuration;
using Microsoft.AspNetCore.WebSockets.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
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

        private static WebSocket _socket;
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _configuration;
        private static ClientSettings _settings;

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
            services.Configure<ClientSettings>(_configuration.GetSection("AppSettings"));
            _serviceProvider = services.BuildServiceProvider();
        }

        public static void Main(string[] args)
        {
            try
            {
                _settings = new ClientSettings();
                ConfigureServices();

                var config = new ConfigurationBuilder()
                    .AddCommandLine(args, commandLineArgs)
                    .Build();

                Console.WriteLine("Press the enter key to connect the server. Waiting to start...");
                Console.ReadKey();

                var socketTask = Task.Run(async () =>
                {
                    await ConnectToWebSocket();
                });
                socketTask.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }

        private static async Task ConnectToWebSocket()
        {
            var settingsConfigurator = _serviceProvider.GetService<IConfigureOptions<ClientSettings>>();
            settingsConfigurator.Configure(_settings);
            WebSocketClient client = new WebSocketClient();
            _socket = await client.ConnectAsync(new Uri(_settings.SocketBaseHostAddress), CancellationToken.None);

            while (_socket.State == WebSocketState.Open)
            {
                var messageDispatcher = new ClientMessageDispatcher(_settings, _socket);
                await messageDispatcher.Listen();
            }
        }
    }
}
