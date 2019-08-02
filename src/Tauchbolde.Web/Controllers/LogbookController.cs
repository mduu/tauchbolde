using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Application.UseCases.Logbook.DeleteUseCase;
using Tauchbolde.Application.UseCases.Logbook.EditUseCase;
using Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase;
using Tauchbolde.Application.UseCases.Logbook.ListAllUseCase;
using Tauchbolde.Application.UseCases.Logbook.NewUseCase;
using Tauchbolde.Application.UseCases.Logbook.PublishUseCase;
using Tauchbolde.Application.UseCases.Logbook.UnpublishUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Helpers;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models.Logbook;

namespace Tauchbolde.Web.Controllers
{
    public class LogbookController : AppControllerBase
    {
        [NotNull] private readonly IPhotoService photoService;
        [NotNull] private readonly ILogger<LogbookController> logger;
        [NotNull] private readonly IMediator mediator;

        public LogbookController(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] IPhotoService photoService,
            [NotNull] IDiverService diverService,
            [NotNull] ILogger<LogbookController> logger,
            [NotNull] IMediator mediator)
            : base(userManager, diverService)
        {
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET /
        public async Task<IActionResult> Index()
        {
            var allowEdit = await GetAllowEdit();
            var allLogbookEntries = await mediator.Send(new ListAllLogbookEntries(allowEdit));
            if (!allLogbookEntries.IsSuccessful)
            {
                ShowErrorMessage("Fehler beim Abfragen aller Logbucheinträge!");
            }
            
            var model = new LogbookListViewModel(
                allLogbookEntries.Payload?.ToList() ?? new List<LogbookEntry>(),
                allowEdit);

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
            var result = await mediator.Send(new DeleteLogbookEntry(id));
            if (!result.IsSuccessful)
            {
                ShowErrorMessage("Fehler beim Löschen des Logbuch Eintrages!");
            }
            else
            {
                ShowSuccessMessage("Logbucheintrag erfolgreich gelöscht.");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Photo(string photoId)
        {
            var photoIdentifier = new PhotoIdentifier(WebUtility.UrlDecode(photoId));
            var photo = await photoService.GetPhotoDataAsync(photoIdentifier);

            if (photo?.Content == null)
            {
                return BadRequest();
            }

            return File(photo.Content, photo.ContentType, photo.Identifier.Filename);
        }

        private async Task<LogbookDetailViewModel> CreateLogbookViewModelAsync(Guid logbookEntryId)
        {
            var logbookEntry = await mediator.Send(new GetLogbookEntryDetails(logbookEntryId));
            if (logbookEntry == null || logbookEntry.Payload == null || !logbookEntry.IsSuccessful)
            {
                return null;
            }

            var allowEdit = await GetAllowEdit();

            return new LogbookDetailViewModel
            {
                AllowEdit = allowEdit,
                Id = logbookEntry.Payload.Id,
                Title = logbookEntry.Payload.Title,
                Teaser = logbookEntry.Payload.TeaserText,
                Text = logbookEntry.Payload.Text,
                ExternalPhotoAlbumUrl = logbookEntry.Payload.ExternalPhotoAlbumUrl,
                TeaserImageUrl = Url.Action("Photo", "Logbook", new {photoId = logbookEntry.Payload.TeaserImage}),
                TeaserThumbImageUrl = Url.Action("Photo", "Logbook", new {photoId = logbookEntry.Payload.TeaserImageThumb}),
                EventTitle = logbookEntry.Payload.EventId != null && logbookEntry.Payload.Event != null
                    ? logbookEntry.Payload.Event.Name
                    : null,
                EventUrl = logbookEntry.Payload.EventId != null
                    ? Url.Action("Details", "Event", new {id = logbookEntry.Payload.EventId})
                    : null,
                IsFavorite = logbookEntry.Payload.IsFavorite,
                IsPublished = logbookEntry.Payload.IsPublished,
                OriginalAuthor = logbookEntry.Payload.OriginalAuthor,
                OriginalAuthorName = logbookEntry.Payload.OriginalAuthor.Realname,
                CreatedAt = logbookEntry.Payload.CreatedAt.ToStringSwissDateTime(),
                EditorAuthor = logbookEntry.Payload.EditorAuthor,
                EditorAuthorName = logbookEntry.Payload.EditorAuthorId != null && logbookEntry.Payload.EditorAuthor != null
                    ? logbookEntry.Payload.EditorAuthor.Realname
                    : null,
                EditedAt = logbookEntry.Payload.ModifiedAt.ToStringSwissDateTime(),
                EditUrl = allowEdit
                    ? Url.Action("Edit", new {id = logbookEntry.Payload.Id})
                    : null,
                PublishUrl = allowEdit
                    ? Url.Action("Publish", new {id = logbookEntry.Payload.Id})
                    : null,
                UnpublishUrl = allowEdit
                    ? Url.Action("Unpublish", new {id = logbookEntry.Payload.Id})
                    : null,
                DeleteUrl = allowEdit
                    ? Url.Action("Delete", new {id = logbookEntry.Payload.Id})
                    : null,
            };
        }

        private async Task<LogbookEditViewModel> CreateLogbookEditViewModelAsync(Guid logbookEntryId)
        {
            var logbookEntry = await mediator.Send(new GetLogbookEntryDetails(logbookEntryId));
            if (logbookEntry == null || logbookEntry.Payload == null || !logbookEntry.IsSuccessful)
            {
                return null;
            }

            return new LogbookEditViewModel
            {
                Id = logbookEntry.Payload.Id,
                CreatedAt = logbookEntry.Payload.CreatedAt,
                Text = logbookEntry.Payload.Text,
                Title = logbookEntry.Payload.Title,
                Teaser = logbookEntry.Payload.TeaserText,
                IsFavorite = logbookEntry.Payload.IsFavorite,
                ExternalPhotoAlbumUrl = logbookEntry.Payload.ExternalPhotoAlbumUrl,
            };
        }

        private async Task<bool> GetAllowEdit() => await GetTauchboldOrAdmin();
    }
}