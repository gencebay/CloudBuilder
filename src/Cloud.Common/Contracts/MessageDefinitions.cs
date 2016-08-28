using Cloud.Common.Core;
using System;

namespace Cloud.Common.Contracts
{
    public class MessageDefinitions
    {
        public Guid ClientId { get; set; }
        public ClientType ClientType { get; set; }
        public Commands[] Commands { get; set; }
        public string Owner { get; set; }
        public Recipient Recipient { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EventName { get; set; }

        public MessageDefinitions()
        {
            Commands = new[] { Common.Commands.Unset };
            CreatedDate = DateTime.Now;
        }
    }
}
