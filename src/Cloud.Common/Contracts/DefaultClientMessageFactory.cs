using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using System;

namespace Cloud.Common.Contracts
{
    public class DefaultClientMessageFactory : IClientMessageFactory
    {
        public byte[] CreateJsonMessage(MessageDefinitions messageDefinitions)
        {
            return messageDefinitions.CreateJson().ToUtf8Bytes();
        }

        public byte[] CreateJsonMessage<T>(string messageResolver, T model)
        {
            var wrappedObj = new { resolver = messageResolver, model = model };
            return wrappedObj.CreateJson().ToUtf8Bytes();
        }

        public byte[] CreateXmlMessage(MessageDefinitions messageDefinitions)
        {
            return messageDefinitions.CreateXml().ToUtf8Bytes();
        }

        public T GetMessage<T>(string serializedObject)
        {
            return serializedObject.FromXml<T>();
        }
    }
}
