using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using System;

namespace Cloud.Common.Contracts
{
    public class DefaultClientMessageFactory : IClientMessageFactory
    {
        public byte[] CreateJsonMessage(CommandDefinitions messageDefinitions)
        {
            return messageDefinitions.CreateJson().ToUtf8Bytes();
        }

        public byte[] CreateXmlMessage(CommandDefinitions messageDefinitions)
        {
            return messageDefinitions.CreateXml().ToUtf8Bytes();
        }

        public T GetMessage<T>(string serializedObject)
        {
            return serializedObject.FromXml<T>();
        }
    }
}
