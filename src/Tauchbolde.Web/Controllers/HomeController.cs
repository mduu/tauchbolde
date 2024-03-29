﻿using System.Diagnostics;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models;
using Tauchbolde.Web.Models.HomeViewModels;
using Tauchbolde.Application.UseCases.Profile.MemberListUseCase;
using Tauchbolde.Driver.SmtpEmail;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.MemberList;

namespace Tauchbolde.Web.Controllers
{
    public class HomeController : AppControllerBase
    {
        [NotNull] private readonly IAppEmailSender emailSender;
        [NotNull] private readonly IMediator mediator;

        public HomeController(
            [NotNull] IMediator mediator,
            [NotNull] IAppEmailSender emailSender)
        {
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

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
            var presenter = new MvcMemberListPresenter();
            var useCaseResult = await mediator.Send(new MemberList(presenter));
            if (!useCaseResult.IsSuccessful)
            {
                ShowErrorMessage("Fehler beim laden der Mitgliederliste!");
            }

            return View(presenter.GetViewModel());
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
        [ValidateAntiForgeryToken]
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
