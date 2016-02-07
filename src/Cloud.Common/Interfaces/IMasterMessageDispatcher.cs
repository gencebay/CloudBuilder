using Cloud.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace Cloud.Common.Interfaces
{
    public interface IMasterMessageDispatcher : IMessageDispatcher
    {
        Task SendMessageAsync(OperationResultContext context);

        Task SendConnectAsJsonAsync(Guid clientId, ClientType clientType);
    }
}
