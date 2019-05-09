using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common;
using Tauchbolde.Common.Domain.Logbook;
using Tauchbolde.Common.Domain.Users;
using Tauchbolde.Common.Model;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models.Logbook;

namespace Tauchbolde.Web.Controllers
{
    public class LogbookController : AppControllerBase
    {
        [NotNull] private readonly ApplicationDbContext context;
        private readonly ILogbookService logbookService;

        public LogbookController(
            [NotNull] ApplicationDbContext context,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] ILogbookService logbookService,
            [NotNull] IDiverService diverService)
            : base(userManager, diverService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
        }

        // GET /
        public async Task<IActionResult> Index()
        {
            var model = new LogbookListViewModel(
                await logbookService.GetAllEntriesAsync(),
                await GetTauchboldOrAdmin());

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

        // GET /new
        [HttpGet]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        public IActionResult New()
        {
            var model = new LogbookEditViewModel
            {
                CreatedAt = DateTime.Now
            };
            
            return View("Edit", model);
        }

        // GET /edit/x
        [HttpGet]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await CreateLogbookEditViewModelAsync(id);
            if (model == null)
            {
                return BadRequest();
            }
                
            return View("Edit", model);
        }

        // POST
        [HttpPost]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        public async Task<IActionResult> Edit(LogbookEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            Stream teaserImageStream = null;
            string teaserImageFilename = null;
            string teaserImageContentType = null;
            if (model.TeaserImage != null && model.TeaserImage.Length > 0)
            {
                teaserImageStream = new MemoryStream();
                await model.TeaserImage.CopyToAsync(teaserImageStream);
                teaserImageFilename = model.TeaserImage.FileName;
                teaserImageContentType = model.TeaserImage.ContentType;
            }

            var currentDiver = await GetDiverForCurrentUserAsync();
            if (currentDiver == null)
            {
                return BadRequest();
            }

            var id = await logbookService.UpsertAsync(new LogbookUpsertModel
            {
                Id = model.Id,
                Text = model.Text,
                Title = model.Title,
                TeaserImage = teaserImageStream,
                TeaserImageFileName = teaserImageFilename,
                TeaserImageContentType = teaserImageContentType,
                Teaser = model.Teaser,
                CreatedAt = model.CreatedAt,
                IsFavorite = model.IsFavorite,
                CurrentDiverId = currentDiver.Id,
                ExternalPhotoAlbumUrl = model.ExternalPhotoAlbumUrl,
            });
            await context.SaveChangesAsync();
            
            ShowSuccessMessage($"Logbucheintrag '{model.Title}' erfolgreich gespeichert.");

            return RedirectToAction("Index", "Logbook");
        }
        
        
        // GET /edit/x
        [HttpGet]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await logbookService.DeleteAsync(id);
            await context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }


        private async Task<LogbookDetailViewModel> CreateLogbookViewModelAsync(Guid logbookEntryId)
        {
            var logbookEntry = await logbookService.FindByIdAsync(logbookEntryId);
            if (logbookEntry == null)
            {
                return null;
            }

            var allowEdit = await GetAllowEdit();
            
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
                EditUrl = allowEdit
                    ? Url.Action("Edit", new { id = logbookEntry.Id })
                    : null,
                DeleteUrl = allowEdit
                    ? Url.Action("Delete", new { id = logbookEntry.Id })
                    : null,
            };
        }
        
        private async Task<LogbookEditViewModel> CreateLogbookEditViewModelAsync(Guid logbookEntryId)
        {
            var logbookEntry = await logbookService.FindByIdAsync(logbookEntryId);
            if (logbookEntry == null)
            {
                return null;
            }

            return new LogbookEditViewModel
            {
                Id = logbookEntry.Id,
                CreatedAt = logbookEntry.CreatedAt,
                Text = logbookEntry.Text,
                Title = logbookEntry.Title,
                Teaser = logbookEntry.TeaserText,
                IsFavorite = logbookEntry.IsFavorite,
                ExternalPhotoAlbumUrl = logbookEntry.ExternalPhotoAlbumUrl,
            };
        }

        private async Task<bool> GetAllowEdit() => await GetTauchboldOrAdmin();
    }
}