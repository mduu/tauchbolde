using Microsoft.AspNetCore.Mvc;

namespace Tauchbolde.Web.Controllers
{
    public class DiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}