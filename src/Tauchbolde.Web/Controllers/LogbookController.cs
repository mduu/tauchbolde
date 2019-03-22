using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DomainServices.Logbook;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Common.DomainServices.Users;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models.Logbook;

namespace Tauchbolde.Web.Controllers
{
    public class LogbookController : AppControllerBase
    {
        private readonly ILogbookService logbookService;
        private readonly IDiverService diverService;

        public LogbookController(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] ILogbookService logbookService,
            [NotNull] IDiverService diverService,
            [NotNull] IDiverRepository diverRepository)
        : base(userManager, diverRepository)
        {
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
        }

        // GET
        public async Task<IActionResult> Index()
        {
            var currentDiver = await GetDiverForCurrentUserAsync();

            var model = new LogbookListViewModel(
                await logbookService.GetAllEntriesAsync(),
                (await GetIsAdmin(currentDiver)) || (await GetIsTauchbold(currentDiver)));

            return View(model);
        }
    }
}