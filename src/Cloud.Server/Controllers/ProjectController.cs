using Cloud.Common.Core;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;

namespace Cloud.Server.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : BaseApiController
    {
        [HttpGet]
        public override string Echo(string message)
        {
            var dateTime = DateTime.Now.ToString();
            return $"Echo from server \"{dateTime}\", Controller Type is: {nameof(ProjectController)}";
        }
    }
}
