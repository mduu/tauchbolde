using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.UseCases.Administration.AddMemberUseCase;
using Tauchbolde.Application.UseCases.Administration.EditRolesUseCase;
using Tauchbolde.Application.UseCases.Administration.GetEditRolesUseCase;
using Tauchbolde.Application.UseCases.Administration.GetMemberManagementUseCase;
using Tauchbolde.Application.UseCases.Administration.SetUpRolesUseCase;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Administration.EditRoles;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Administration.MemberManagement;
using Tauchbolde.SharedKernel;
using Tauchbolde.Web.Core;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireAdministrator)]
    public class AdminController : AppControllerBase
    {
        [NotNull] private readonly IMediator mediator;

        public AdminController([NotNull] IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> MemberManagement()
        {
            var presenter = new MvcMemberManagementPresenter();
            var useCaseResult = await mediator.Send(new GetMemberManagement(presenter));
            if (!useCaseResult.IsSuccessful)
            {
                var msgMap = new Dictionary<ResultCategory, string>
                {
                    {ResultCategory.AccessDenied, "Zugriff verweigert!"},
                    {ResultCategory.GeneralFailure, "Allgemeiner Fehler aufgetreten!"}
                };

                ShowErrorMessage(msgMap[useCaseResult.ResultCategory]);
                return RedirectToAction(nameof(Index));
            }

            return View(presenter.GetViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddMembers(string userName, string firstname, string lastname)
        {
            var useCaseResult = await mediator.Send(new AddMember(userName, firstname, lastname));

            var msgMap = new Dictionary<ResultCategory, Action>
            {
                {ResultCategory.Success, () => ShowSuccessMessage("Mitglied erfolgreich hinzugefügt!")},
                {ResultCategory.NotFound, () => ShowErrorMessage("Kein registrierter Benutzer mit Benutzername '{username}' gefunden!")},
                {ResultCategory.AccessDenied, () => ShowErrorMessage("Sie haben keine Berechtigung um neue Mitglieder hinzu zu fügen!")},
            };
            msgMap[useCaseResult.ResultCategory]();

            if (!string.IsNullOrWhiteSpace(useCaseResult.Payload))
            {
                ShowWarningMessage(useCaseResult.Payload);
            }

            return RedirectToAction(nameof(MemberManagement));
        }

        [HttpGet]
        public async Task<IActionResult> EditRoles(string userName)
        {
            var presenter = new MvcEditRolesPresenter();
            var useCaseResult = await mediator.Send(new GetEditRoles(userName, presenter));
            if (!useCaseResult.IsSuccessful)
            {
                ShowErrorMessage($"Benutzer mit username {userName} nicht gefunden!");
                return RedirectToAction("MemberManagement");
            }
            
            return View(presenter.GetViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(string userName, string[] roles)
        {
            var useCaseResult = await mediator.Send(new EditRoles(userName, roles));

            var msgMap = new Dictionary<ResultCategory, Action>
            {
                {ResultCategory.Success, () => ShowSuccessMessage($"Rollenzuweisung an {userName} erfolgreich: {string.Join(",", roles)}")},
                {ResultCategory.NotFound, () => ShowErrorMessage($"Fehler beim Ändern der Rollen für Benutzer [{userName}]")},
            };
            msgMap[useCaseResult.ResultCategory]();

            return RedirectToAction("MemberManagement");
        }

        [HttpGet]
        public async Task<IActionResult> ConfigureRoles()
        {
            var useCaseResult = await mediator.Send(new SetUpRoles());

            return useCaseResult.IsSuccessful
                ? Ok()
                : StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}