using Cloud.Common.Contracts;
using System.Collections.Generic;

namespace Cloud.Common.Core
{
    public static class RandomMessageGenerator
    {
        public static List<CommandDefinitions> Messages = new List<CommandDefinitions>
        {
            new CommandDefinitions
            {
                Commands = new[] { CommandType.Build, CommandType.Test },
                Owner = "Server1",
                Recipient = new Recipient
                {
                    AssemblyName = "Sample.ConsoleApp",
                    Version = "1.0.0-*"
                }
            },

            new CommandDefinitions
            {
                Commands = new [] { CommandType.Test },
                Owner = "Server1",
                Recipient = new Recipient
                {
                    AssemblyName = "Sample.TestProject",
                    Version = "1.0.0-*"
                }
            },

            new CommandDefinitions
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
