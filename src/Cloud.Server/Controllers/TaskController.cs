using Cloud.Common.Contracts;
using Cloud.Common.Core;
using Cloud.Common.Interfaces;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Cloud.Server.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : BaseApiController
    {
        private readonly ConcurrentBag<IMessageDispatcher> _dispatcherBag;

        public TaskController(ConcurrentBag<IMessageDispatcher> dispatcherBag)
        {
            _dispatcherBag = dispatcherBag;
        }

        [HttpGet]
        public override string Echo(string message)
        {
            var dateTime = DateTime.Now.ToString();
            return $"Echo from server \"{dateTime}\", Controller Type is: {nameof(TaskController)}";
        }

        [HttpPost("Completed")]
        public void Completed([FromBody]OperationResultContext context)
        {

        }
    }
}
