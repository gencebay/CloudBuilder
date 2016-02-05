using Cloud.Common.Contracts;
using Cloud.Common.Core;

namespace Cloud.Common.Interfaces
{
    public interface IClientMessageFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string CreateMessage(MessageDefinitions messageDefinitions);
    }
}