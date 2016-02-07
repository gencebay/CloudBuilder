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
        byte[] CreateJsonMessage(CommandDefinitions messageDefinitions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDefinitions"></param>
        /// <returns></returns>
        byte[] CreateXmlMessage(CommandDefinitions messageDefinitions);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        T GetMessage<T>(string serializedObject);
    }
}