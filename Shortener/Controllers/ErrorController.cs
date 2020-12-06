using Microsoft.AspNetCore.Mvc;
using Shortener.Models;
using System.Diagnostics;

namespace Shortener.Controllers
{
    public class ErrorController : Controller
    {
        public new IActionResult NotFound()
        {
            return View(new NotFoundViewModel { });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
