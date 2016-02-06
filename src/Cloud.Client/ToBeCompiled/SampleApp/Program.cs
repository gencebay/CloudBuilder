using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;

namespace SampleApp
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;
        public Program(IApplicationEnvironment env)
        {
            var services = new ServiceCollection();

            _serviceProvider = services.BuildServiceProvider();
        }

        public void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            Console.Read();
        }
    }
}
