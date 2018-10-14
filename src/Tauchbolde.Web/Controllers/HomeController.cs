using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Web.Models;
using Tauchbolde.Web.Models.AboutViewModels;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDiverService diverService;
        private readonly IDiverRepository diverRepository;

        public HomeController(
            IDiverService diverService,
            IDiverRepository diverRepository
        )
        {
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var model = new AboutViewModel
            {
                Members = await diverService.GetMembersAsync(diverRepository)
            };

           return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
