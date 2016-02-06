using Cloud.Common.Contracts;
using Cloud.Common.DependencyInjection;
using Cloud.Common.Interfaces;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Routing;
using Microsoft.Dnx.Compilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using Moq;
using System;
using System.IO;

namespace Cloud.Common.UnitTests
{
    public abstract class BaseUnitTest
    {
        private IServiceCollection _serviceCollection;
        protected IServiceProvider ServiceProvider { get; private set; }

        public BaseUnitTest()
        {
            _serviceCollection = new ServiceCollection();
            PrepareServiceProvider();
        }

        private T GetOptions<T>(Action<IServiceCollection> action = null) where T : class, new()
        {
            var serviceProvider = PrepareServiceProvider(action);
            return serviceProvider.GetRequiredService<IOptions<T>>().Value;
        }

        protected IServiceProvider PrepareServiceProvider(Action<IServiceCollection> action = null)
        {
            _serviceCollection.AddTransient<ILoggerFactory, LoggerFactory>();
            _serviceCollection.AddSingleton<IClientMessageFactory, DefaultClientMessageFactory>();

            _serviceCollection.AddMvc();

            if (action != null)
            {
                action(_serviceCollection);
            }

            ServiceProvider = _serviceCollection.BuildServiceProvider();
            return ServiceProvider;
        }

        protected void AddDnxServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(Mock.Of<ILibraryManager>());
            serviceCollection.AddSingleton(Mock.Of<ILibraryExporter>());
            serviceCollection.AddSingleton(Mock.Of<ICompilerOptionsProvider>());
            serviceCollection.AddSingleton(Mock.Of<IAssemblyLoadContextAccessor>());
            serviceCollection.AddSingleton(Mock.Of<IHostingEnvironment>());
            var applicationEnvironment = new Mock<IApplicationEnvironment>();

            // ApplicationBasePath is used to set up a PhysicalFileProvider which requires a real directory.
            applicationEnvironment.SetupGet(e => e.ApplicationBasePath)
                .Returns(Directory.GetCurrentDirectory());

            serviceCollection.AddSingleton(applicationEnvironment.Object);
        }

        protected ActionContext CreateActionContext(HttpContext context = null)
        {
            if (context != null)
                return new ActionContext(context, new RouteData(), new ActionDescriptor());

            var request = Mock.Of<HttpRequest>();
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(c => c.Request)
                           .Returns(request);

            return new ActionContext(httpContext.Object, new RouteData(), new ActionDescriptor());
        }

        protected HttpContext GetHttpContext(string contentType = "application/json")
        {
            var request = new Mock<HttpRequest>();
            var headers = new Mock<IHeaderDictionary>();
            request.SetupGet(f => f.ContentType).Returns(contentType);

            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            return httpContext.Object;
        }
    }
}
