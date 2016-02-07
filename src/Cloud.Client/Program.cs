using Cloud.Client.Core;
using Cloud.Common.Configuration;
using Microsoft.AspNetCore.WebSockets.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Client
{
    public class Program
    {
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
            var clientId = Guid.NewGuid();
            var uri = $"{_settings.SocketBaseHostAddress}?clientId=" + clientId;
            _socket = await client.ConnectAsync(new Uri(uri), CancellationToken.None);

            while (_socket.State == WebSocketState.Open)
            {
                var messageDispatcher = new ClientMessageDispatcher(_settings, _socket, clientId);
                await messageDispatcher.Listen();
            }
        }
    }
}
