using Microsoft.AspNetCore.Mvc;
using RemoteAuthenticator.Models;
using System.Diagnostics;

namespace RemoteAuthenticator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Authorize([FromQuery] string callbackPath)
        {
            var fullPath = string.Format("{0}{1}", Request.GetTypedHeaders().Referer!.ToString(), callbackPath.Substring(1));
            return View("Authorize", fullPath);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}