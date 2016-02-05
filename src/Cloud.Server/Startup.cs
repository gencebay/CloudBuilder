using Cloud.Server.Core;
using Cloud.Server.Interfaces;
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
using Cloud.Server.Middleware;
using Cloud.Server.Extensions;
using Cloud.Common.Interfaces;
using Cloud.Common.Contracts;

namespace Cloud.Server
{
    public class Startup
    {
        private readonly ConcurrentBag<IMessageDispatcher> _dispatcherBag;

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _dispatcherBag = new ConcurrentBag<IMessageDispatcher>();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Dependencies
            services.AddSingleton<IClientMessageFactory, DefaultClientMessageFactory>();
            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
            services.AddInstance(typeof(ConcurrentBag<IMessageDispatcher>), _dispatcherBag);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Verbose);
            loggerFactory.AddDebug();

            var logger = loggerFactory.CreateLogger(nameof(Startup));

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseWebSockets();

            app.UseSocket();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
