using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QLGPLX.Data;
using QLGPLX.Models;

namespace QLGPLX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QlgplxContext _context;

        public HomeController(ILogger<HomeController> logger, QlgplxContext qlgplxContext)
        {
            _logger = logger;
            _context = qlgplxContext;
        }

        public IActionResult Index()
        {
            congdan user = _context.congdans.FirstOrDefault(t => t.HoTen == "Trần Thị Bình");
            Console.WriteLine("--------------");
            Console.WriteLine(user.CCCD);
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
