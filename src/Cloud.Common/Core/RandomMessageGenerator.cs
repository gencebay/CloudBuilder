﻿using Cloud.Common.Contracts;
using Microsoft.Extensions.PlatformAbstractions;
using System;
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
                    AssemblyName = "SampleApp",
                    Version = "1.0.0-*"
                }
            },

            new CommandDefinitions
            {
                Commands = new [] { CommandType.Test },
                Owner = "Server1",
                Recipient = new Recipient
                {
                    AssemblyName = "SampleApp.Test",
                    Version = "1.0.0-*"
                }
            }
        };
    }
}
