using Cloud.Common.Contracts;
using Cloud.Common.Core;
using Cloud.Common.Interfaces;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

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
        public async Task Completed([FromBody]OperationResultContext context)
        {
            if (context.State)
            {
                // Notify log to Master Server
                var masterSocket = _dispatcherBag.Where(x => x.ClientType == ClientType.Master).FirstOrDefault();
                if (masterSocket != null)
                    await masterSocket.SendMessageAsync(context);

                // Ready for new task
                // Send random message
                var clientSocket = _dispatcherBag.Where(x => x.ClientId == context.ClientId).FirstOrDefault();
                if (clientSocket != null)
                    clientSocket.SendMessage(new { dummy = true });
            }
        }
    }
}
