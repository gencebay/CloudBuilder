using Cloud.Common.Core;
using System;

namespace Cloud.Common.Contracts
{
    public class OperationResultContext
    {
        public Guid ClientId { get; set; }
        public string ResultInfo { get; set; }
        public Commands Command { get; set; }
        public OperationResultState State { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}
