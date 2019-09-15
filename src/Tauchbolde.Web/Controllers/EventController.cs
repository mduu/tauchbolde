using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tauchbolde.Application.OldDomainServices.Events;
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.UseCases.Event.ChangeParticipationUseCase;
using Tauchbolde.Application.UseCases.Event.DeleteCommentUseCase;
using Tauchbolde.Application.UseCases.Event.EditCommentUseCase;
using Tauchbolde.Application.UseCases.Event.EditEventUseCase;
using Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase;
using Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase;
using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;
using Tauchbolde.Application.UseCases.Event.NewCommentUseCase;
using Tauchbolde.Application.UseCases.Event.NewEventUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.InterfaceAdapters.Event;
using Tauchbolde.InterfaceAdapters.Event.Details;
using Tauchbolde.InterfaceAdapters.Event.List;
using Tauchbolde.SharedKernel;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models.EventViewModels;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class EventController : AppControllerBase
    {
        [NotNull] private readonly IEventService eventService;
        [NotNull] private readonly IDiverService diverService;
        [NotNull] private readonly IMediator mediator;

        public EventController(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] IEventService eventService,
            [NotNull] IDiverService diverService,
            [NotNull] IMediator mediator)
            : base(userManager, diverService)
        {
            this.eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
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
            Event detailsForEvent = null;
            if (id.HasValue)
            {
                detailsForEvent = await eventService.GetByIdAsync(id.Value);
                if (detailsForEvent == null)
                {
                    return BadRequest("Event does not exists!");
                }
            }

            var currentUser = await GetDiverForCurrentUserAsync();
            if (currentUser == null)
            {
                return BadRequest();
            }

            var model = new EventEditViewModel
            {
                Id = detailsForEvent?.Id ?? Guid.Empty,
                Name = detailsForEvent?.Name ?? "",
                Location = detailsForEvent?.Location ?? "",
                MeetingPoint = detailsForEvent?.MeetingPoint ?? "",
                Description = detailsForEvent?.Description ?? "",
                Organisator = (detailsForEvent?.Organisator ?? currentUser).User.UserName,
                StartTime = detailsForEvent?.StartTime ?? DateTime.Today.AddDays(1).AddHours(19),
                EndTime = detailsForEvent?.EndTime,
            };

            return View(model);
        }


        // POST: Event/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, EventEditModel model)
        {
            var currentDiver = await GetDiverForCurrentUserAsync();

            EventEditViewModel CreateViewModelFromEditModel(EventEditModel editModel) =>
                new EventEditViewModel
                {
                    Id = id,
                    Name = editModel.Name,
                    Description = editModel.Description,
                    Location = editModel.Location,
                    MeetingPoint = editModel.MeetingPoint,
                    StartTime = editModel.StartTime,
                    EndTime = editModel.EndTime,
                    Organisator = currentDiver.Fullname
                };

            ActionResult CreateGeneralEditErrorResult(Exception ex = null)
            {
                ModelState.AddModelError("",
                    "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    $"see your system administrator. Message: {ex?.Message}, {ex?.InnerException?.Message}");
                return View(CreateViewModelFromEditModel(model));
            }

            if (!ModelState.IsValid)
            {
                return View(CreateViewModelFromEditModel(model));
            }

            try
            {
                var isNew = id == Guid.Empty;
                if (isNew)
                {
                    var newResult = await mediator.Send(
                        new NewEvent(
                            GetCurrentUserName(),
                            model.StartTime, 
                            model.EndTime,
                            model.Name,
                            model.Location,
                            model.MeetingPoint,
                            model.Description));

                    if (!newResult.IsSuccessful)
                    {
                        return CreateGeneralEditErrorResult();
                    }

                    id = newResult.Payload;
                }
                else
                {
                    var editResult = await mediator.Send(
                        new EditEvent(
                            GetCurrentUserName(),
                            id,
                            model.StartTime,
                            model.EndTime,
                            model.Name,
                            model.Location,
                            model.MeetingPoint,
                            model.Description));

                    if (!editResult.IsSuccessful)
                    {
                        return CreateGeneralEditErrorResult();
                    }
                }

                return RedirectToAction("Details", new {id});
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

        private static IEnumerable<SelectListItem> GetBuddyTeamNames()
        {
            return BuddyTeamNames.Names.Select(n => new SelectListItem {Text = n});
        }
    }
}