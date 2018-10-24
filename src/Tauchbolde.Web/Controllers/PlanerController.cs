using Microsoft.AspNetCore.Mvc;

namespace Tauchbolde.Web.Controllers
{
    public class PlanerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}