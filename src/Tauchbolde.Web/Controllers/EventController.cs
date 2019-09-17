using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.UseCases.Event.ChangeParticipationUseCase;
using Tauchbolde.Application.UseCases.Event.DeleteCommentUseCase;
using Tauchbolde.Application.UseCases.Event.EditCommentUseCase;
using Tauchbolde.Application.UseCases.Event.EditEventUseCase;
using Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase;
using Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase;
using Tauchbolde.Application.UseCases.Event.GetEventEditDetailsUseCase;
using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;
using Tauchbolde.Application.UseCases.Event.NewCommentUseCase;
using Tauchbolde.InterfaceAdapters.Event;
using Tauchbolde.InterfaceAdapters.Event.Details;
using Tauchbolde.InterfaceAdapters.Event.EditDetails;
using Tauchbolde.InterfaceAdapters.Event.List;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Extensions;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models.EventViewModels;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class EventController : AppControllerBase
    {
        [NotNull] private readonly IDiverService diverService;
        [NotNull] private readonly IMediator mediator;

        public EventController(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] IDiverService diverService,
            [NotNull] IMediator mediator)
            : base(userManager, diverService)
        {
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET: Event
        public async Task<ActionResult> Index()
        {
            var presenter = new MvcEventListPresenter();
            var useCaseResult = await mediator.Send(new GetEventList(presenter));
            if (!useCaseResult.IsSuccessful)
            {
                return StatusCode(500);
            }

            return View(presenter.GetViewModel());
        }

        // GET: Event/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var presenter = new MvcEventDetailPresenter();
            var result = await mediator.Send(new GetEventDetails(id, presenter, User.Identity.Name));
            if (!result.IsSuccessful)
            {
                return NotFound();
            }

            return View(presenter.GetViewModel());
        }


        // GET: Event/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            var presenter = new MvcEventEditDetailsPresenter();
            var useCaseResult = await mediator.Send(new GetEventEditDetails(GetCurrentUserName(), id, presenter));
            if (!useCaseResult.IsSuccessful)
            {
                if (useCaseResult.ResultCategory == ResultCategory.NotFound)
                {
                    return NotFound();
                }

                return StatusCode(500);
            }

            return View(presenter.GetViewModel());
        }


        // POST: Event/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, MvcEventEditDetailsViewModel model)
        {
            ActionResult CreateGeneralEditErrorResult(Exception ex = null)
            {
                ModelState.AddModelError("",
                    "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    $"see your system administrator. Message: {ex?.Message}, {ex?.InnerException?.Message}");
                return View(model);
            }

            if (!DateTime.TryParse(model.StartTime, out var startDateTime))
            {
                ModelState.AddModelError(nameof(model.StartTime), "Ungültiges Datum!");
            }

            DateTime? endDateTime = null;
            if (model.EndTime != null)
            {
                if (DateTime.TryParse(model.EndTime, out var eTime))
                {
                    endDateTime = eTime;
                }
                else
                {
                    ModelState.AddModelError(nameof(model.EndTime), "Ungültige Endzeit!");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var editResult = await mediator.Send(
                    new EditEvent(
                        GetCurrentUserName(),
                        id,
                        startDateTime,
                        endDateTime,
                        model.Title,
                        model.Location,
                        model.MeetingPoint,
                        model.Description));

                if (!editResult.IsSuccessful)
                {
                    return CreateGeneralEditErrorResult();
                }

                return RedirectToAction("Details", new { id = editResult.Payload});
            }
            catch (Exception ex)
            {
                return CreateGeneralEditErrorResult(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangeParticipation([Bind(Prefix = "Participations")] ChangeParticipantViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Details(model.EventId);
            }

            var currentUser = await diverService.FindByUserNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return StatusCode(400, "No current user would be found!");
            }

            var result = await mediator.Send(
                new ChangeParticipation(
                    currentUser.User.UserName,
                    model.EventId,
                    model.CurrentUserStatus,
                    model.CurrentUserCountPeople,
                    model.CurrentUserNote,
                    model.CurrentUserBuddyTeamName
                ));

            if (!result.IsSuccessful)
            {
                return result.ResultCategory == ResultCategory.NotFound
                    ? NotFound()
                    : StatusCode(500);
            }

            return RedirectToAction("Details", new {id = model.EventId});
        }

        /// <summary>
        /// Download a .ical file of the specified event.
        /// </summary>
        public async Task<IActionResult> Ical(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Event!");
            }

            var presenter = new ExportIcalPresenter();
            var useCaseResult = await mediator.Send(new ExportIcalStream(id, presenter));
            if (!useCaseResult.IsSuccessful)
            {
                return useCaseResult.ResultCategory == ResultCategory.NotFound
                    ? NotFound()
                    : StatusCode(500);
            }

            return File(presenter.GetIcalStream(), "text/calendar", presenter.GetDownloadFilename());
        }

        /// <summary>
        /// Adds a new comment to an event.
        /// </summary>
        /// <param name="eventId">ID of the event to add the comment to.</param>
        /// <param name="newCommentText">The text to add as a comment.</param>
        public async Task<IActionResult> AddComment(Guid eventId, string newCommentText)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new {id = eventId});
            }

            var currentUser = await diverService.FindByUserNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return StatusCode(400, "No current user would be found!");
            }

            var comment = await mediator.Send(new NewComment(eventId, currentUser.Id, newCommentText));
            if (!comment.IsSuccessful)
            {
                ShowErrorMessage(
                    comment.ResultCategory == ResultCategory.NotFound
                        ? "Fehler beim Speichern des neuen Kommentares: Aktivität nicht gefunden!"
                        : "Fehler beim Speichern des neuen Kommentares!");

                return RedirectToAction("Details", "Event", new {Id = eventId});
            }

            ShowSuccessMessage("Kommentar erfolgreich gespeichert.");
            return RedirectToAction("Details", new {id = eventId});
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(Guid eventId, Guid commentId, string commentText)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new {id = eventId});
            }

            var currentUser = await diverService.FindByUserNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return StatusCode(400, "No current user would be found!");
            }

            var interactorResult = await mediator.Send(new EditComment(commentId, commentText));
            if (!interactorResult.IsSuccessful)
            {
                ShowErrorMessage("Fehler beim Speichern des Kommentares!");
            }

            return RedirectToAction("Details", new {id = eventId});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(Guid deleteEventId, Guid deleteCommentId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new {id = deleteEventId});
            }

            var currentUser = await diverService.FindByUserNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return StatusCode(400, "No curren user would be found!");
            }

            var result = await mediator.Send(new DeleteComment(deleteCommentId, currentUser.Id));
            if (!result.IsSuccessful)
            {
                ShowErrorMessage("Fehler beim Löschen des Kommentars!");
            }

            return RedirectToAction("Details", new {id = deleteEventId});
        }
    }
}