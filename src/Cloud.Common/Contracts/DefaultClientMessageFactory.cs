using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using System;

namespace Cloud.Common.Contracts
{
    public class DefaultClientMessageFactory : IClientMessageFactory
    {
        public byte[] CreateMessage(CommandDefinitions messageDefinitions)
        {
            return messageDefinitions.CreateXml().ToUtf8Bytes();
        }

        public ArraySegment<byte> CreateMessageSegment(CommandDefinitions messageDefinitions)
        {
            return new ArraySegment<byte>(messageDefinitions.CreateXml().ToUtf8Bytes());
        }

        public T GetMessage<T>(string serializedObject)
        {
            return serializedObject.FromXml<T>();
        }
    }
}
