using Cloud.Server.Middleware;
using Microsoft.AspNet.Builder;

namespace Cloud.Server.Extensions
{
    public static class BuilderExtensions
    {
        public static void UseSocket(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SocketRequestMiddleware>();
        }
    }
}
