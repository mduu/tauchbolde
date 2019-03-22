using Microsoft.AspNetCore.Mvc;

namespace Tauchbolde.Web.Controllers
{
    public class LogbookController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}