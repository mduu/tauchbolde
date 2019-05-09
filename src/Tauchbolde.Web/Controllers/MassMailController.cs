using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common;
using Tauchbolde.Web.Core;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Web.Models.MassMail;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Common.Domain;
using Tauchbolde.Common.Domain.Users;

namespace Tauchbolde.Web.Controllers
{

    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class MassMailController : AppControllerBase
    {
        private readonly IMassMailService massMailService;
        [NotNull] private readonly IDiverService diverService;

        public MassMailController(
            IMassMailService massMailService,
            UserManager<IdentityUser> userManager,
            [NotNull] IDiverService diverService)
            : base (userManager, diverService)
        {
            this.massMailService = massMailService ?? throw new System.ArgumentNullException(nameof(massMailService));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
        }

        // GET: /MassMail/
        public async Task<IActionResult> Index()
        {
            var tauchbolde = await diverService.GetMembersAsync();

            return base.View(new MassMailViewModel
            {
                TauchboldeEmailReceiver = massMailService.CreateReceiverString(tauchbolde)
            });
        }
    }
}
