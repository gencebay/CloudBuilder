using Cloud.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace Cloud.Common.Interfaces
{
    public interface IClientMessageDispatcher
    {
        Guid ClientId { get; }
        Task Listen();
        Task DoWork(CommandDefinitions command);
    }
}