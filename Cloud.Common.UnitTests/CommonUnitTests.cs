using Cloud.Server.Controllers;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Cloud.Common.UnitTests
{
    public class CommonUnitTests: BaseUnitTest
    {
        public CommonUnitTests()
            :base()
        {

        }

        [Fact]
        public void FirstUnitTestAsTrue()
        {
            Assert.True(true);
        }

        [Fact]
        public void ControllerCreation()
        {
            var httpContext = GetHttpContext();

            var services = GetServiceProvider(s =>
            {
                s.AddTransient<ILoggerFactory, LoggerFactory>();
                s.AddScoped<IHttpContextAccessor>(x => new HttpContextAccessor { HttpContext = httpContext });
            });

            var projectController = new ProjectController();
            projectController.ActionContext = CreateActionContext(httpContext);

            Assert.NotNull(projectController.Echo("message body"));
        }
    }
}
