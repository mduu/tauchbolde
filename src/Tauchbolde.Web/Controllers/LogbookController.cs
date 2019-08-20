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
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Application.UseCases.Logbook.DeleteUseCase;
using Tauchbolde.Application.UseCases.Logbook.EditUseCase;
using Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase;
using Tauchbolde.Application.UseCases.Logbook.ListAllUseCase;
using Tauchbolde.Application.UseCases.Logbook.NewUseCase;
using Tauchbolde.Application.UseCases.Logbook.PublishUseCase;
using Tauchbolde.Application.UseCases.Logbook.UnpublishUseCase;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.InterfaceAdapters;
using Tauchbolde.InterfaceAdapters.Logbook.Details;
using Tauchbolde.InterfaceAdapters.Logbook.Edit;
using Tauchbolde.InterfaceAdapters.Logbook.ListAll;
using Tauchbolde.InterfaceAdapters.TextFormatting;
using Tauchbolde.Web.Core;

namespace Tauchbolde.Web.Controllers
{
    public class LogbookController : AppControllerBase
    {
        [NotNull] private readonly IPhotoService photoService;
        [NotNull] private readonly ILogger<LogbookController> logger;
        [NotNull] private readonly IMediator mediator;
        [NotNull] private readonly ITextFormatter textFormatter;
        private readonly IRelativeUrlGenerator relativeUrlGenerator;
        [NotNull] private readonly ILogbookDetailsUrlGenerator logbookDetailsUrlGenerator;

        public LogbookController(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] IPhotoService photoService,
            [NotNull] IDiverService diverService,
            [NotNull] ILogger<LogbookController> logger,
            [NotNull] IMediator mediator,
            [NotNull] ITextFormatter textFormatter,
            [NotNull] IRelativeUrlGenerator relativeUrlGenerator,
            [NotNull] ILogbookDetailsUrlGenerator logbookDetailsUrlGenerator)
            : base(userManager, diverService)
        {
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
            this.relativeUrlGenerator = relativeUrlGenerator ?? throw new ArgumentNullException(nameof(relativeUrlGenerator));
            this.logbookDetailsUrlGenerator = logbookDetailsUrlGenerator ?? throw new ArgumentNullException(nameof(logbookDetailsUrlGenerator));
        }

        // GET /
        public async Task<IActionResult> Index()
        {
            var allowEdit = await GetAllowEdit();
            var presenter = new MvcListLogbookPresenter(allowEdit, textFormatter);
            var interactorResult = await mediator.Send(new ListAllLogbookEntries(allowEdit, presenter));
            if (!interactorResult.IsSuccessful)
            {
                ShowErrorMessage("Fehler beim Abfragen aller Logbucheinträge!");
            }

            return View(presenter.GetViewModel());
        }

        // GET /detail/x
        public async Task<IActionResult> Detail(Guid id)
        {
            var allowEdit = await GetAllowEdit();
            var presenter = new MvcLogbookDetailsPresenter(relativeUrlGenerator, logbookDetailsUrlGenerator);
            var interactorResult = await mediator.Send(new GetLogbookEntryDetails(id, presenter, allowEdit));
            if (!interactorResult.IsSuccessful)
            {
                ShowErrorMessage("Fehler beim laden der Daten des Logbucheintrages!");
            }

            return View(presenter.GetViewModel());
        }

        // GET /new
        [HttpGet]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        public IActionResult New()
        {
            var model = new LogbookEditViewModel(null, false, null, "", null, "", null, DateTime.Now);

            return View("Edit", model);
        }

        // GET /edit/x
        [HttpGet]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var presenter = new MvcLogbookEditDetailsPresenter();
            var interactorResult = await mediator.Send(new GetLogbookEntryDetails(id, presenter, true));
            if (!interactorResult.IsSuccessful)
            {
                return NotFound();
            }

            return View("Edit", presenter.GetViewModel());
        }

        // POST
        [HttpPost]
        [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LogbookEditEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", new LogbookEditViewModel(
                    model.Id,
                    model.IsFavorite,
                    model.TeaserImage,
                    model.Title,
                    model.Teaser,
                    model.Text,
                    model.ExternalPhotoAlbumUrl,
                    model.CreatedAt));
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

        private async Task<bool> GetAllowEdit() => await GetTauchboldOrAdmin();
    }
}