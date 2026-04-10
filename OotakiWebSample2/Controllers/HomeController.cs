using Microsoft.AspNetCore.Mvc;
using OotakiWebSample2.Models;
using System.Diagnostics;

namespace OotakiWebSample2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService _service;

        public HomeController(IService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _service.GetScreenAsync(1, 1);
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
