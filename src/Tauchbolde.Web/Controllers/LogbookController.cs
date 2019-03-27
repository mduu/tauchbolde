using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common;
using Tauchbolde.Common.DomainServices.Logbook;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models.Logbook;

namespace Tauchbolde.Web.Controllers
{
    public class LogbookController : AppControllerBase
    {
        private readonly ILogbookService logbookService;

        public LogbookController(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] ILogbookService logbookService,
            [NotNull] IDiverRepository diverRepository)
            : base(userManager, diverRepository)
        {
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
        }

        // GET /
        public async Task<IActionResult> Index()
        {
            var model = new LogbookListViewModel(
                await logbookService.GetAllEntriesAsync(),
                await GetAllowEditAsync());

            return View(model);
        }

        // GET /detail/x
        public async Task<IActionResult> Detail(Guid id)
        {
            var model = await CreateLogbookViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        private async Task<LogbookDetailViewModel> CreateLogbookViewModelAsync(Guid logbookEntryId)
        {
            var allowEdit = await GetAllowEditAsync();

            var logbookEntry = await logbookService.FindByIdAsync(logbookEntryId);
            if (logbookEntry == null)
            {
                return null;
            }

            return new LogbookDetailViewModel
            {
                AllowEdit = allowEdit,
                Id = logbookEntry.Id,
                Title = logbookEntry.Title,
                Teaser = logbookEntry.TeaserText,
                Text = logbookEntry.Text,
                ExternalPhotoAlbumUrl = logbookEntry.ExternalPhotoAlbumUrl,
                EventTitel = logbookEntry.EventId != null && logbookEntry.Event != null
                    ? logbookEntry.Event.Name
                    : null,
                EventUrl = logbookEntry.EventId != null
                    ? Url.Action("Details", "Event", new {id = logbookEntry.EventId})
                    : null,
                IsFavorite = logbookEntry.IsFavorite,
                OriginalAuthor = logbookEntry.OriginalAuthor,
                OriginalAuthorName = logbookEntry.OriginalAuthor.Realname,
                CreatedAt = logbookEntry.CreatedAt.ToStringSwissDateTime(),
                EditorAuthor = logbookEntry.EditorAuthor,
                EditorAuthorName = logbookEntry.EditorAuthorId != null && logbookEntry.EditorAuthor != null
                    ? logbookEntry.EditorAuthor.Realname
                    : null,
                EditedAt = logbookEntry.ModifiedAt.ToStringSwissDateTime(),
            };
        }

        private async Task<bool> GetAllowEditAsync()
        {
            var currentDiver = await GetDiverForCurrentUserAsync();
            return await GetIsAdmin(currentDiver) || await GetIsTauchbold(currentDiver);
        }
    }
}