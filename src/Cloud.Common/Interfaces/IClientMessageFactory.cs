using Cloud.Common.Contracts;
using System;

namespace Cloud.Common.Interfaces
{
    public interface IClientMessageFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        byte[] CreateJsonMessage(MessageDefinitions messageDefinitions);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        byte[] CreateJsonMessage<T>(string messageResolver, T model);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDefinitions"></param>
        /// <returns></returns>
        byte[] CreateXmlMessage(MessageDefinitions messageDefinitions);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        T GetMessage<T>(string serializedObject);
    }
}