using Cloud.Common.Core;
using System;

namespace Cloud.Common.Contracts
{
    public class MessageDefinitions
    {
        public CommandType[] Commands { get; set; }
        public string Owner { get; set; }
        public Recipient Recipient { get; set; }
        public DateTime CreatedDate { get; set; }

        public MessageDefinitions()
        {
            Commands = new[] { CommandType.Build, CommandType.Test };
            CreatedDate = DateTime.Now;
        }
    }
}
