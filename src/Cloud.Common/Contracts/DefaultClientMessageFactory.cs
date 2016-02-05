using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using System;

namespace Cloud.Common.Contracts
{
    public class DefaultClientMessageFactory : IClientMessageFactory
    {
        public string CreateMessage(MessageDefinitions messageDefinitions)
        {
            return messageDefinitions.CreateXmlMessage();
        }
    }
}
