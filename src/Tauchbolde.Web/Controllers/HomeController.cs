using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Web.Models;
using Tauchbolde.Web.Models.AboutViewModels;
using Tauchbolde.Common.Repositories;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Common;
using Tauchbolde.Web.Models.HomeViewModels;
using Tauchbolde.Web.Core;
using Tauchbolde.Common.DomainServices.SMTPSender;

namespace Tauchbolde.Web.Controllers
{
    public class HomeController : AppControllerBase
    {
        private readonly IDiverService diverService;
        private readonly IDiverRepository diverRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAppEmailSender emailSender;

        public HomeController(
            IDiverService diverService,
            IDiverRepository diverRepository,
            UserManager<IdentityUser> userManager,
            IAppEmailSender emailSender
        )
        {
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
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
            var rand = new Random();
            var viewModel = new ContactViewModel
            {
                NumberA = rand.Next(1, 10),
                NumberB = rand.Next(1, 10),
            };

            return View(viewModel);
        }
        
        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = model.NumberA + model.NumberB;
                if (model.Result != result)
                {
                    ModelState.AddModelError(nameof(model.Result), "Falsches Resultat!");
                }
                else
                {
                    emailSender.Send(
                        "Webmaster",
                        "marc@marcduerst.com",

                         "Tauchbolde Kontaktformular",
                        model.Text,
                        model.YourEmail,
                        model.YourName);

                    ShowSuccessMessage("Vielen Dank für Deine Nachricht. Du wirst von uns höhren.");
                    return RedirectToAction("Indedx", "Home");
                }
            }
            
            return View(model);
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
