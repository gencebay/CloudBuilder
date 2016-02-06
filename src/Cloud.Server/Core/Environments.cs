using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.Server.Core
{
    public static class Environments
    {
        public static Random Random { get; set; }
        static Environments()
        {
            Random = new Random();
        }
    }
}
