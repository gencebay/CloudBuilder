using Cloud.Common.DependencyInjection;
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
        public BaseUnitTest()
        {

        }

        private T GetOptions<T>(Action<IServiceCollection> action = null) where T : class, new()
        {
            var serviceProvider = GetServiceProvider(action);
            return serviceProvider.GetRequiredService<IOptions<T>>().Value;
        }

        protected IServiceProvider GetServiceProvider(Action<IServiceCollection> action = null)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddMvc();

            if (action != null)
            {
                action(serviceCollection);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
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
