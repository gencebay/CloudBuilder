using Cloud.Common.Contracts;
using System.Collections.Generic;

namespace Cloud.Common.Core
{
    public static class RandomMessageGenerator
    {
        public static List<MessageDefinitions> Messages = new List<MessageDefinitions>
        {
            new MessageDefinitions
            {
                Commands = new[] { CommandType.Build },
                Owner = "Server1",
                Recipient = new Recipient
                {
                    AssemblyName = "Sample.ConsoleApp",
                    Version = "1.0.0-*"
                }
            },

            new MessageDefinitions
            {
                Owner = "Server1",
                Recipient = new Recipient
                {
                    AssemblyName = "Sample.AnotherApp",
                    Version = "1.0.0-*"
                }
            },
        };
    }
}
