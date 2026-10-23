using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using _24_1639BasicProgrammingLanguage.Services;

namespace _24_1639BasicProgrammingLanguage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProgramCatalog _catalog;

        public HomeController(ProgramCatalog catalog) => _catalog = catalog;

        public IActionResult Index()
        {
            ViewBag.Categories = _catalog.Categories;
            ViewBag.Programs = _catalog.Programs;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
