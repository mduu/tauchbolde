using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Common;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.UserProfileModels;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Web.Core;

namespace Tauchbolde.Web.Controllers
{
    [Route("/profil")]
    [Authorize(Policy = PolicyNames.RequireTauchbold)]
    public class UserProfileController : AppControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IDiverRepository diverRepository;
        private readonly IDiverService diverService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserProfileController(
            ApplicationDbContext context,
            IDiverRepository diverRepository,
            IDiverService diverService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        // GET: /profil/<id>
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var currentDiver = await diverRepository.FindByUserNameAsync(User.Identity.Name);
            if (currentDiver == null)
            {
                return StatusCode(400, "No curren user would be found!");
            }

            var isAdmin = await userManager.IsInRoleAsync(currentDiver.User, Rolenames.Administrator);

            var member = await diverService.GetMemberAsync(diverRepository, id);
            if (member == null)
            {
                return StatusCode(400, "Member (Diver) not found!");
            }

            return View(new ReadProfileModel
            {
                AllowEdit = member.Id == currentDiver.Id || isAdmin,
                Profile = member,
            });
        }

        // GET: /profil/edit
        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var currentDiver = await diverRepository.FindByUserNameAsync(User.Identity.Name);
            if (currentDiver == null)
            {
                return StatusCode(400, "No curren user would be found!");
            }

            var isAdmin = await userManager.IsInRoleAsync(currentDiver.User, Rolenames.Administrator);

            var member = await diverService.GetMemberAsync(diverRepository, id);
            if (member == null)
            {
                return StatusCode(400, "Member (Diver) not found!");
            }

            if (member.Id != currentDiver.Id && !isAdmin)
            {
                return Forbid();
            }

            return base.View(new WriteProfileModel
            {
                Profile = member,
            });
        }

        // GET: /profil/edit
        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, WriteProfileModel model)
        {
            if (ModelState.IsValid)
            {
                await diverService.UpdateUserProfil(diverRepository, model.Profile);
                await context.SaveChangesAsync();
                
                ShowSuccessMessage("Profil wurde gespeichert.");
                return RedirectToAction("Index", new { id = model.Profile.User.UserName });
            }

            ShowErrorMessage("Fehler beim Speichern aufgetreten!");
        
            return View(new WriteProfileModel
            {
                Profile = model.Profile,
            });
        }
    }
}
