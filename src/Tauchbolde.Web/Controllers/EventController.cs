using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tauchbolde.Common;
using Tauchbolde.Common.Domain.Events;
using Tauchbolde.Common.Domain.Users;
using Tauchbolde.Entities;
using Tauchbolde.DataAccess;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Models.EventViewModels;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class EventController : AppControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IParticipationService participationService;
        private readonly IEventService eventService;
        [NotNull] private readonly IDiverService diverService;

        public EventController(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] ApplicationDbContext context,
            [NotNull] IParticipationService participationService,
            [NotNull] IEventService eventService,
            [NotNull] IDiverService diverService)
        :base(userManager, diverService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.participationService = participationService ?? throw new ArgumentNullException(nameof(participationService));
            this.eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
        }

        // GET: Event
        public async Task<ActionResult> Index()
        {
            var model = new EventListViewModel
            {
                UpcommingEvents = await eventService.GetUpcomingEventsAsync(),
            };

            return View(model);
        }

        // GET: Event/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var detailsForEvent = await eventService.GetByIdAsync(id);
            if (detailsForEvent == null)
            {
                return BadRequest("Event does not exists!");
            }

            var currentDiver = await diverService.FindByUserNameAsync(User.Identity.Name);
            if (currentDiver == null)
            {
                return StatusCode(400, "No current user would be found!");
            }

            var existingParticipation = await participationService.GetExistingParticipationAsync(currentDiver, id);
            var allowEdit = detailsForEvent.OrganisatorId == currentDiver.Id;
            var model = new EventViewModel
            {
                Event = detailsForEvent,
                CurrentDiver = currentDiver,
                BuddyTeamNames = GetBuddyTeamNames(),   
                AllowEdit = allowEdit,
                ChangeParticipantViewModel = new ChangeParticipantViewModel
                {
                    EventId = detailsForEvent.Id,
                    Note = existingParticipation?.Note,
                    CountPeople = existingParticipation?.CountPeople ?? 1,
                    Status = existingParticipation?.Status ?? ParticipantStatus.None,
                    BuddyTeamName = existingParticipation?.BuddyTeamName,
                }
            };

            return View(model);
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
                OriginalEvent = detailsForEvent,
                BuddyTeamNames = GetBuddyTeamNames(),
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
        public async Task<ActionResult> Edit(Guid id, EventEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentDiver = await GetDiverForCurrentUserAsync();
                    var evt = new Event
                    {
                        Id = model.Id,
                        Name = model.Name,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        Description = model.Description,
                        Location = model.Location,
                        MeetingPoint = model.MeetingPoint,
                        OrganisatorId = currentDiver.Id,
                    };

                    var persistedEvent = await eventService.UpsertEventAsync(evt, currentDiver);
                    await context.SaveChangesAsync();

                    return RedirectToAction("Details", new { persistedEvent.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                         $"see your system administrator. Message: {ex.Message}, {ex.InnerException.Message}");
                    model.BuddyTeamNames = GetBuddyTeamNames();
                    return View(model);
                }
            }

            EventEditViewModel viewModel;
            if (id != Guid.Empty)
            {
                var detailsForEvent = await eventService.GetByIdAsync(id);
                if (detailsForEvent == null)
                {
                    return NotFound("Event does not exists!");
                }

                viewModel = new EventEditViewModel { OriginalEvent = detailsForEvent };
            }
            else
            {
                viewModel = model;
            }

            viewModel.BuddyTeamNames = GetBuddyTeamNames();

            return View(viewModel);
        }

        /// <summary>
        /// Changes the participant state of a user.
        /// </summary>
        /// <param name="model">The data to change.</param>
        /// <seealso cref="ChangeParticipantViewModel"/>
        /// <seealso cref="IParticipationService"/>
        [HttpPost]
        public async Task<ActionResult> ChangeParticipation([Bind(Prefix = "ChangeParticipantViewModel")]ChangeParticipantViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await diverService.FindByUserNameAsync(User.Identity.Name);
                if (currentUser == null)
                {
                    return StatusCode(400, "No current user would be found!");
                }

                await participationService.ChangeParticipationAsync(currentUser, model.EventId, model.Status, model.CountPeople, model.Note, model.BuddyTeamName);
                await context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = model.EventId });
            }

            return await Details(model.EventId);
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

            var stream = await eventService.CreateIcalForEventAsync(id);

            return File(stream, "text/calendar");
        }

        /// <summary>
        /// Adds a new comment to an event.
        /// </summary>
        /// <param name="eventId">ID of the event to add the comment to.</param>
        /// <param name="newCommentText">The text to add as a comment.</param>
        public async Task<IActionResult> AddComment(Guid eventId, string newCommentText)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await diverService.FindByUserNameAsync(User.Identity.Name);
                if (currentUser == null)
                {
                    return StatusCode(400, "No current user would be found!");
                }

                var comment = await eventService.AddCommentAsync(eventId, newCommentText, currentUser);
                if (comment != null)
                {
                    await context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("", "Fehler beim Speichern des Kommentares!");
                }

            }

            return RedirectToAction("Details", new { id = eventId });
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(Guid eventId, Guid commentId, string commentText)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await diverService.FindByUserNameAsync(User.Identity.Name);
                if (currentUser == null)
                {
                    return StatusCode(400, "No current user would be found!");
                }

                var comment = await eventService.EditCommentAsync(commentId, commentText, currentUser);
                if (comment != null)
                {
                    await context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("", "Fehler beim Speichern des Kommentares!");
                }

            }

            return RedirectToAction("Details", new { id = eventId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(Guid deleteEventId, Guid deleteCommentId)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await diverService.FindByUserNameAsync(User.Identity.Name);
                if (currentUser == null)
                {
                    return StatusCode(400, "No curren user would be found!");
                }

                await eventService.DeleteCommentAsync(deleteCommentId, currentUser);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = deleteEventId });
        }

        private static IEnumerable<SelectListItem> GetBuddyTeamNames()
        {
            return BuddyTeamNames.Names.Select(n => new SelectListItem { Text = n });
        }
    }
}