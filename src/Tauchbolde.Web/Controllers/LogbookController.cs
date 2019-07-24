using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.OldDomainServices.Logbook;
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.UseCases.Logbook.EditUseCase;
using Tauchbolde.Application.UseCases.Logbook.NewUseCase;
using Tauchbolde.Application.UseCases.Logbook.PublishUseCase;
using Tauchbolde.Application.UseCases.Logbook.UnpublishUseCase;
using Tauchbolde.Domain.Helpers;
using Tauchbolde.Driver.DataAccessSql;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models.Logbook;

namespace Tauchbolde.Web.Controllers
{
    public class LogbookController : AppControllerBase
    {
        [NotNull] private readonly ApplicationDbContext context;
        [NotNull] private readonly ILogbookService logbookService;
        [NotNull] private readonly ILogger<LogbookController> logger;
        [NotNull] private readonly IMediator mediator;

        public LogbookController(
            [NotNull] ApplicationDbContext context,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] ILogbookService logbookService,
            [NotNull] IDiverService diverService,
            [NotNull] ILogger<LogbookController> logger,
            [NotNull] IMediator mediator)
            : base(userManager, diverService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET /
        public async Task<IActionResult> Index()
        {
            var allowEdit = await GetAllowEdit();
            var model = new LogbookListViewModel(
                await logbookService.GetAllEntriesAsync(allowEdit),
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

            if (!model.Id.HasValue)
            {
                await mediator.Send(new NewLogbookEntry(
                    currentDiver.Id,
                    model.Title,
                    model.Teaser,
                    model.Text,
                    model.IsFavorite,
                    teaserImageStream,
                    teaserImageFilename,
                    teaserImageContentType,
                    model.ExternalPhotoAlbumUrl,
                    null));
            }
            else
            {
                await mediator.Send(new EditLogbookEntry(
                    model.Id.Value,
                    currentDiver.Id,
                    model.Title,
                    model.Teaser,
                    model.Text,
                    model.IsFavorite,
                    teaserImageStream,
                    teaserImageFilename,
                    teaserImageContentType,
                    model.ExternalPhotoAlbumUrl));
            }

            ShowSuccessMessage($"Logbucheintrag '{model.Title}' erfolgreich gespeichert.");

            return RedirectToAction("Index", "Logbook");
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        public async Task<IActionResult> Publish(Guid id)
        {
            var publishLogbookEntry = new PublishLogbookEntry(id);
            var result = await mediator.Send(publishLogbookEntry);
            if (!result)
            {
                ShowErrorMessage("Fehler beim Publizieren des Logbucheintrages!");
            }

            ShowSuccessMessage("Logbucheintrag erfolgreich publiziert.");
            return RedirectToAction("Detail", "Logbook", new {id});
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        public async Task<IActionResult> Unpublish(Guid id)
        {
            var unpublishLogbookEntry = new UnpublishLogbookEntry(id);
            var result = await mediator.Send(unpublishLogbookEntry);
            if (!result)
            {
                ShowErrorMessage("Fehler beim nicht mehr publizieren des Logbuch Eintrages!");
            }
            else
            {
                ShowSuccessMessage("Logbucheintrag erfolgreich nicht mehr publiziert.");
            }

            return RedirectToAction("Detail", "Logbook", new {id});
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

        [HttpGet]
        public async Task<IActionResult> Photo(string photoId)
        {
            var photoIdentifier = new PhotoIdentifier(WebUtility.UrlDecode(photoId));
            var photo = await logbookService.GetPhotoDataAsync(photoIdentifier);

            if (photo?.Content == null)
            {
                return BadRequest();
            }

            return File(photo.Content, photo.ContentType, photo.Identifier.Filename);
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
                TeaserImageUrl = Url.Action("Photo", "Logbook", new {photoId = logbookEntry.TeaserImage}),
                TeaserThumbImageUrl = Url.Action("Photo", "Logbook", new {photoId = logbookEntry.TeaserImageThumb}),
                EventTitle = logbookEntry.EventId != null && logbookEntry.Event != null
                    ? logbookEntry.Event.Name
                    : null,
                EventUrl = logbookEntry.EventId != null
                    ? Url.Action("Details", "Event", new {id = logbookEntry.EventId})
                    : null,
                IsFavorite = logbookEntry.IsFavorite,
                IsPublished = logbookEntry.IsPublished,
                OriginalAuthor = logbookEntry.OriginalAuthor,
                OriginalAuthorName = logbookEntry.OriginalAuthor.Realname,
                CreatedAt = logbookEntry.CreatedAt.ToStringSwissDateTime(),
                EditorAuthor = logbookEntry.EditorAuthor,
                EditorAuthorName = logbookEntry.EditorAuthorId != null && logbookEntry.EditorAuthor != null
                    ? logbookEntry.EditorAuthor.Realname
                    : null,
                EditedAt = logbookEntry.ModifiedAt.ToStringSwissDateTime(),
                EditUrl = allowEdit
                    ? Url.Action("Edit", new {id = logbookEntry.Id})
                    : null,
                PublishUrl = allowEdit
                    ? Url.Action("Publish", new {id = logbookEntry.Id})
                    : null,
                UnpublishUrl = allowEdit
                    ? Url.Action("Unpublish", new {id = logbookEntry.Id})
                    : null,
                DeleteUrl = allowEdit
                    ? Url.Action("Delete", new {id = logbookEntry.Id})
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