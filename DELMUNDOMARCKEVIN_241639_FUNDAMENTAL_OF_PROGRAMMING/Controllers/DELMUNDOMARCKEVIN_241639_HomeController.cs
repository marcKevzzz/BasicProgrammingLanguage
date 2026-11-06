using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_HomeController : Controller
    {
        private readonly ProgramCatalog _catalog;

        public DELMUNDOMARCKEVIN_241639_HomeController(ProgramCatalog catalog) => _catalog = catalog;

        public IActionResult Index()
        {
            return View(new DELMUNDOMARCKEVIN_241639_HomeIndexModel(_catalog.Categories, _catalog.Programs));
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
