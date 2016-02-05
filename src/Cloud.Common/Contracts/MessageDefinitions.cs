using System;

namespace Cloud.Common.Contracts
{
    public class MessageDefinitions
    {
        public string Command { get; set; }
        public string Owner { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}
