using Microsoft.AspNetCore.Mvc;
using System;

namespace Cloud.Common.Core
{
    public class BaseApiController : Controller
    {
        [HttpGet]
        public virtual string Echo(string message)
        {
            var dateTime = DateTime.Now.ToString();
            return $"Echo from server: \"{dateTime}\"";
        }
    }
}
