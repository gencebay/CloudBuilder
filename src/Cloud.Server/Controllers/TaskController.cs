using Cloud.Common.Contracts;
using Cloud.Common.Core;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.Server.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : BaseApiController
    {
        private readonly IMasterMessageDispatcher _masterMessageDispatcher;
        private readonly ConcurrentBag<IMessageDispatcher> _dispatcherBag;

        bool MasterReady
        {
            get
            {
                if (_masterMessageDispatcher == null)
                    return false;

                return true;
            }
        }

        public TaskController(IMasterMessageDispatcher masterMessageDispatcher,
            ConcurrentBag<IMessageDispatcher> dispatcherBag)
        {
            _masterMessageDispatcher = masterMessageDispatcher;
            _dispatcherBag = dispatcherBag;
        }

        [HttpGet]
        public override string Echo(string message)
        {
            var dateTime = DateTime.Now.ToString();
            return $"Echo from server \"{dateTime}\", Controller Type is: {nameof(TaskController)}";
        }

        [HttpPost("RunTask")]
        public async Task RunTask([FromBody]ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dispatcher = _dispatcherBag.Where(x => x.ClientId == model.ClientId).FirstOrDefault();
                if (dispatcher != null)
                {
                    // Start random task
                    await dispatcher.SendMessageAsync();
                }
            }
        }

        [HttpPost("Completed")]
        public async Task Completed([FromBody]OperationResultContext context)
        {
            // Notify log to Master Server
            await _masterMessageDispatcher.SendMessageAsync(context);

            // Ready for new task order a new random message
            var clientSocket = _dispatcherBag.Where(x => x.ClientId == context.ClientId).FirstOrDefault();
            if (clientSocket != null)
                await clientSocket.SendMessageAsync();
        }
    }
}
