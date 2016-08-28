using Cloud.Client.Core;
using Cloud.Common.Configuration;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Client
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _configuration;
        private static ClientSettings _settings;

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
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
                Console.WriteLine("Waiting for order!");
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
            ClientWebSocket client = new ClientWebSocket();
            var clientId = Guid.NewGuid();
            var uri = $"{_settings.SocketBaseHostAddress}?clientId=" + clientId;
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);

            while (client.State == WebSocketState.Open)
            {
                var messageDispatcher = new ClientMessageDispatcher(_settings, client, clientId);
                await messageDispatcher.Listen();
            }
        }
    }
}
