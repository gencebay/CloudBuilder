using System;

namespace Cloud.Common.Core
{
    public static class Globals
    {
        public static Random Random { get; set; }
        static Globals()
        {
            Random = new Random();
        }

        public const string ClientId = "ClientId";
        public const string Owner = "Owner";
        public const string Master = "Master";
    }

    public static class Resolvers
    {
        public const string NotifyConnected = "NotifyConnected";
    }
}
