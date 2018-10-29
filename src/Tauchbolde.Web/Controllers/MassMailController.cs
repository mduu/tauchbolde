using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common;
using Tauchbolde.Web.Core;
using System.Threading.Tasks;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.MassMail;
using Tauchbolde.Common.DomainServices;

namespace Tauchbolde.Web.Controllers
{

    [Authorize(Policy = PolicyNames.RequireTauchbold)]
    [Authorize(Policy = PolicyNames.RequireAdministrator)]
    public class MassMailController : AppControllerBase
    {
        private readonly IDiverRepository diverRepository;
        private readonly IMassMailService massMailService;

        public MassMailController(
            IDiverRepository diverRepository,
            IMassMailService massMailService)
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
