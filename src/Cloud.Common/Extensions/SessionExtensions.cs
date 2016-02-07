using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using System;

namespace Cloud.Common.Extensions
{
    public static class SessionExtensions
    {
        public static Guid? GetOrSetGuid(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
                return Guid.NewGuid();

            return new Guid(data);
        }
    }
}
