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
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Common;

namespace Tauchbolde.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDiverService diverService;
        private readonly IDiverRepository diverRepository;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(
            IDiverService diverService,
            IDiverRepository diverRepository,
            UserManager<IdentityUser> userManager
        )
        {
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var isTauchbold = false;

            if (User?.Identity != null && !string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                var currentDiver = await diverRepository.FindByUserNameAsync(User.Identity.Name);
                if (currentDiver != null)
                {
                    isTauchbold = await userManager.IsInRoleAsync(currentDiver.User, Rolenames.Tauchbold);
                }
            }
        
            var model = new AboutViewModel
            {
                Members = await diverService.GetMembersAsync(diverRepository),
                IsTauchbold = isTauchbold,
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
