using la_mia_pizzeria_static.CustomLogger;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    public class HomeController : Controller
    {
        private ICustomLogger _myLogger;
        public HomeController(ICustomLogger n)
        {
            _myLogger = n;
        }

        [HttpGet]
        public IActionResult index()
        {
            _myLogger.WriteLog("sono nella home");
            return View();
        }
   

    
        public IActionResult UserIndex()
        {
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