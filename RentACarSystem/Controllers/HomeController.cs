using RentACarSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace RentACarSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index(string search)
        {
            GlobalProperties.IP_Address = HttpContext.Connection.RemoteIpAddress.ToString();
            if (!string.IsNullOrEmpty(search))
                if (search.Length > 0)
                    return View("Search");
            return View();
        }
        [HttpGet]
        public IActionResult Booking()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Gallery()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PriceList()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Services()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Testimonials()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
