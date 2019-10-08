using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models;
using Tauchbolde.Web.Models.AboutViewModels;
using Tauchbolde.Web.Models.HomeViewModels;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Types;
using Tauchbolde.Driver.SmtpEmail;

namespace Tauchbolde.Web.Controllers
{
    public class HomeController : AppControllerBase
    {
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly IAppEmailSender emailSender;
        
        public HomeController(
            [NotNull] IDiverRepository diverRepository,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] IAppEmailSender emailSender)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        [EnableCors("AllowTwitter")]
        public IActionResult Index() => View();

        [Authorize]
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(GlobalConstants.AuthCookieName);
            return RedirectToAction("Index");
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
                Members = await diverRepository.GetAllTauchboldeUsersAsync(),
                IsTauchbold = isTauchbold,
            };

           return View(model);
        }

        [ValidateAntiForgeryToken]
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
                    emailSender.SendAsync(
                        "Webmaster",
                        "marc@marcduerst.com",
                        "Tauchbolde Kontaktformular",
                        model.Text,
                        model.YourEmail,
                        model.YourName);

                    ShowSuccessMessage("Vielen Dank für Deine Nachricht. Du wirst von uns höhren.");
                    return RedirectToAction("Index", "Home");
                }
            }
            
            return View(model);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
