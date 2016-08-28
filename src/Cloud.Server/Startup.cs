using Cloud.Common.Configuration;
using Cloud.Common.Contracts;
using Cloud.Common.Interfaces;
using Cloud.Server.Core;
using Cloud.Server.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;

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
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _dispatcherBag = new ConcurrentBag<IMessageDispatcher>();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddXmlDataContractSerializerFormatters();

            services.AddLogging();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSession();

            // Configurations
            services.Configure<ServerSettings>(Configuration.GetSection("AppSettings"));

            services.AddSingleton(_ => services);

            // Dependencies
            services.AddSingleton<IClientMessageFactory, DefaultClientMessageFactory>();
            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
            services.AddSingleton(typeof(ConcurrentBag<IMessageDispatcher>), _dispatcherBag);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();

            app.UseSession();

            //app.UseWebSockets();

            app.UseSocket();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseKestrel()
                .UseIISIntegration()
                .Build();

            host.Run();
        }
    }
}
