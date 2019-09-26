using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Web.Core;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Web.Models.MassMail;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.OldDomainServices;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class MassMailController : AppControllerBase
    {
        private readonly IMassMailService massMailService;
        [NotNull] private readonly IDiverRepository diverRepository;

        public MassMailController(
            IMassMailService massMailService,
            UserManager<IdentityUser> userManager,
            [NotNull] IDiverRepository diverRepository)
        {
            this.massMailService = massMailService ?? throw new System.ArgumentNullException(nameof(massMailService));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }

        // GET: /MassMail/
        public async Task<IActionResult> Index()
        {
            // TODO Convert this to a usecase interactor and presenter
            var tauchbolde = await diverRepository.GetAllTauchboldeUsersAsync();

            return base.View(new MassMailViewModel
            {
                TauchboldeEmailReceiver = massMailService.CreateReceiverString(tauchbolde)
            });
        }
    }
}
