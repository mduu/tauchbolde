using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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