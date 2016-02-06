using Cloud.Common.Core;
using System;

namespace Cloud.Common.Contracts
{
    public class OperationResultContext
    {
        public Guid ClientId { get; set; }
        public string ResultInfo { get; set; }
        public CommandType Command { get; set; }
        public bool State { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}
