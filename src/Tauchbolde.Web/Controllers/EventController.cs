using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tauchbolde.Common;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.EventViewModels;
using Tauchbolde.Web.Services;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireTauchbold)]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDiverRepository _diverRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IParticipationService _participationService;
        private readonly IEventService _eventService;

        public EventController(
            ApplicationDbContext context,
            IDiverRepository diverRepository,
            IEventRepository eventRepository,
            IParticipationService participationService,
            IEventService eventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _participationService = participationService ?? throw new ArgumentNullException(nameof(participationService));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        }

        // GET: Event
        public async Task<ActionResult> Index()
        {
            var model = new EventListViewModel
            {
                UpcommingEvents = await _eventRepository.GetUpcommingEventsAsync(),
            };

            return View(model);
        }

        // GET: Event/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var detailsForEvent = await _eventRepository.FindByIdAsync(id);
            if (detailsForEvent == null)
            {
                return BadRequest("Event does not exists!");
            }

            var currentUser = await _diverRepository.FindByUserNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return StatusCode(400, "No curren user would be found!");
            }

            var existingParticipation = await _participationService.GetExistingParticipationAsync(currentUser, id);
            var allowEdit = detailsForEvent == null || detailsForEvent?.OrganisatorId == currentUser.Id;
            var model = new EventViewModel
            {
                Event = detailsForEvent,
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
                detailsForEvent = await _eventRepository.FindByIdAsync(id.Value);
                if (detailsForEvent == null)
                {
                    return BadRequest("Event does not exists!");
                }
            }

            var currentUser = await this.GetCurrentUserAsync(_diverRepository);
            if (currentUser == null) {
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
                    var currentDiver = await this.GetCurrentUserAsync(_diverRepository);
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

                    var persistedEvent = await _eventService.UpsertEventAsync(_eventRepository, evt);
                    await _context.SaveChangesAsync();

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
                var detailsForEvent = await _eventRepository.FindByIdAsync(id);
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

        // GET: Event/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Event/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
                var currentUser = await _diverRepository.FindByUserNameAsync(User.Identity.Name);
                if (currentUser == null)
                {
                    return StatusCode(400, "No curren user would be found!");
                }

                await _participationService.ChangeParticipationAsync(currentUser, model.EventId, model.Status, model.CountPeople, model.Note, model.BuddyTeamName);

                return RedirectToAction("Details", new { id = model.EventId });
            }

            return await Details(model.EventId);
        }

        public async Task<IActionResult> Ical(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Event!");
            }

            var stream = await _eventService.CreateIcalForEvent(id, _eventRepository);

            return File(stream, "text/calendar");
        }

        private static IEnumerable<SelectListItem> GetBuddyTeamNames()
        {
            return BuddyTeamNames.Names.Select(n => new SelectListItem { Text = n });
        }
    }
}