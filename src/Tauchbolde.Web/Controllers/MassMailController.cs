using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common;
using Tauchbolde.Web.Core;
using System.Threading.Tasks;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.MassMail;
using Tauchbolde.Common.DomainServices;
using Microsoft.AspNetCore.Identity;

namespace Tauchbolde.Web.Controllers
{

    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class MassMailController : AppControllerBase
    {
        private readonly IDiverRepository diverRepository;
        private readonly IMassMailService massMailService;

        public MassMailController(
            IDiverRepository diverRepository,
            IMassMailService massMailService,
            UserManager<IdentityUser> userManager)
            : base (userManager, diverRepository)
        {
            this.diverRepository = diverRepository ?? throw new System.ArgumentNullException(nameof(diverRepository));
            this.massMailService = massMailService ?? throw new System.ArgumentNullException(nameof(massMailService));
        }

        // GET: /MassMail/
        public async Task<IActionResult> Index()
        {
            var tauchbolde = await diverRepository.GetAllTauchboldeUsersAsync();

            return base.View(new MassMailViewModel
            {
                TauchboldeEmailReceiver = massMailService.CreateReceiverString(tauchbolde)
            });
        }
    }
}
