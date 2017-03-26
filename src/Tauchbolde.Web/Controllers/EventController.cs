using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.Net.Http.Headers;
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
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IParticipationService _participationService;
        private readonly IEventService _eventService;

        public EventController(
            ApplicationDbContext context,
            IApplicationUserRepository applicationUserRepository,
            IEventRepository eventRepository,
            IParticipationService participationService,
            IEventService eventService)
        {
            if (applicationUserRepository == null) { throw new ArgumentNullException(nameof(applicationUserRepository)); }
            if (eventRepository == null) throw new ArgumentNullException(nameof(eventRepository));
            if (participationService == null) { throw new ArgumentNullException(nameof(participationService)); }
            if (eventService == null) throw new ArgumentNullException(nameof(eventService));

            _applicationUserRepository = applicationUserRepository;
            _eventRepository = eventRepository;
            _participationService = participationService;
            _eventService = eventService;
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

            var currentUser = await _applicationUserRepository.FindByUserNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return StatusCode(400, "No curren user would be found!");
            }

            var existingParticipation = await _participationService.GetExistingParticipationAsync(currentUser, id);

            var model = new EventViewModel
            {
                Event = detailsForEvent,
                BuddyTeamNames = GetBuddyTeamNames(),
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

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Event/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            var detailsForEvent = await _eventRepository.FindByIdAsync(id);
            if (detailsForEvent == null)
            {
                return BadRequest("Event does not exists!");
            }

            var model = new EventEditViewModel
            {
                OriginalEvent = detailsForEvent,
                BuddyTeamNames = GetBuddyTeamNames(),
                Id = detailsForEvent.Id,
                Name = detailsForEvent.Name,
                Location = detailsForEvent.Location,
                MeetingPoint = detailsForEvent.MeetingPoint,
                Description = detailsForEvent.Description,
                Organisator = (detailsForEvent.Organisator ?? await this.GetCurrentUserAsync(_applicationUserRepository)).UserName,
                StartTime = detailsForEvent.StartTime,
                EndTime = detailsForEvent.EndTime
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
                    // TODO: Add update logic here


                    return RedirectToAction("Details", new { id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        $"see your system administrator. Message: {ex.Message}");
                }
            }

            var detailsForEvent = await _eventRepository.FindByIdAsync(id);
            if (detailsForEvent == null)
            {
                return BadRequest("Event does not exists!");
            }

            var viewModel = new EventEditViewModel()
            {
                OriginalEvent = detailsForEvent,
                BuddyTeamNames = GetBuddyTeamNames(),
            };


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
                var currentUser = await _applicationUserRepository.FindByUserNameAsync(User.Identity.Name);
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