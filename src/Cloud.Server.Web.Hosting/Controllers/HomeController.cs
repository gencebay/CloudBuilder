using Cloud.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;

namespace Cloud.Server.Web.Hosting.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // HttpContext.Session.GetOrSetGuid("ClientId");
            var sessionId = HttpContext.Session.Id;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
