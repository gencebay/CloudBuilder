using Cloud.Common.Configuration;
using Cloud.Common.Contracts;
using Cloud.Common.Interfaces;
using Cloud.Server.Core;
using Cloud.Server.Extensions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace Cloud.Server
{
    public class Startup
    {
        private readonly IMasterMessageDispatcher _masterMessageDispatcher;
        private readonly ConcurrentBag<IMessageDispatcher> _dispatcherBag;

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json")
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

            // Adds a default in-memory implementation of IDistributedCache
            services.AddCaching();

            // Api explorer
            services.AddSwaggerGen();

            // Configurations
            services.Configure<ServerSettings>(Configuration.GetSection("AppSettings"));

            // Session
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".Cloud.Server";
            });

            services.AddSingleton(_ => services);

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

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseSession();

            app.UseWebSockets();

            app.UseSocket();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwaggerGen();

            app.UseSwaggerUi();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
