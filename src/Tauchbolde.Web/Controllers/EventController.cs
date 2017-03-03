using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel;
using Tauchbolde.Common;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.EventViewModels;

namespace Tauchbolde.Web.Controllers
{

    [Authorize(Policy = PolicyNames.RequireTauchbold )]
    public class EventController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IParticipationService _participationService;

        public EventController(
            ApplicationDbContext context,
            IApplicationUserRepository applicationUserRepository,
            IEventRepository eventRepository,
            IParticipationService participationService)
        {
            if (applicationUserRepository == null) { throw new ArgumentNullException(nameof(applicationUserRepository)); }
            if (eventRepository == null) throw new ArgumentNullException(nameof(eventRepository));
            if (participationService == null) { throw new ArgumentNullException(nameof(participationService)); }

            _applicationUserRepository = applicationUserRepository;
            _eventRepository = eventRepository;
            _participationService = participationService;
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
            var model = new EventViewModel
            {
                Event = await _eventRepository.FindByIdAsync(id),
                BuddyTeamNames = BuddyTeamNames.Names.Select(n => new SelectListItem{ Text = n}),
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
        /// [HttpPost]
        [Authorize(Policy = PolicyNames.RequireTauchbold)]
        public async Task<ActionResult> ChangeParticipation(ChangeParticipantViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _applicationUserRepository.FindByUserNameAsync(User.Identity.Name);
                if (currentUser == null)
                {
                    return StatusCode(400, "No curren user would be found!");
                }

                await _participationService.ChangeParticipationAsync(currentUser, model.ExistingParticipantId, model.EventId, model.Status, model.CountPeople, model.Note, model.BuddyTeamName);

                return Json(new { success = true });
            }

            return Json(new {success = false, ModelState});
        }
    }
}