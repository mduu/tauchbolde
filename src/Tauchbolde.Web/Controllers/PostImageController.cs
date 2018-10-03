using System;
using Microsoft.AspNetCore.Mvc;

namespace Tauchbolde.Web.Controllers
{
    public class PostImageController : Controller
    {
        [ResponseCache(CacheProfileName = "OneDay", VaryByQueryKeys = new[] { "id" })]
        public IActionResult Thumbnail(Guid id)
        {
            return View();
        }
    }
}