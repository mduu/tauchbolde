using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Web.Core;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.UseCases.Administration;
using Tauchbolde.InterfaceAdapters.Administration.GetMassMailDetails;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class MassMailController : AppControllerBase
    {
        [NotNull] private readonly IMediator mediator;

        public MassMailController([NotNull] IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET: /MassMail/
        public async Task<IActionResult> Index()
        {
            var presenter = new MvcGetMassMailDetailsPresenter();
            var useCaseResult = await mediator.Send(new GetMassMailDetails(presenter));
            if (!useCaseResult.IsSuccessful)
            {
                ShowErrorMessage("Fehler beim ermitteln der Mitgliederdaten!");
            }

            return base.View(presenter.GetViewModel());
        }
    }
}
