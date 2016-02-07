using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System;

namespace SampleApp
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;

        public Program()
        {
            var services = new ServiceCollection();
            services.AddTransient<ILoggerFactory, LoggerFactory>();
            _serviceProvider = services.BuildServiceProvider();
        }

        public void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            Console.Read();
        }
    }
}
