using System;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tauchbolde.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Common.Repositories;
using System.Collections.Generic;
using Tauchbolde.Web.Models.AdminViewModels;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Web.Core;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireAdministrator)]
    public class AdminController : AppControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IDiverRepository diverRepository;
        private readonly IDiverService diverService;

        public AdminController(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            IDiverRepository diverRepository,
            IDiverService diverService)
            : base(userManager, diverRepository)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MemberManagement()
        {
            var profiles = (await diverRepository.GetAllDiversAsync()).ToArray();

            var members = new List<MemberViewModel>();
            foreach (var member in profiles)
            {
                members.Add(await CreateMemberViewModel(member));
            }

            var allMembers = await diverRepository.GetAllAsync();
            var allUsers = userManager.Users
                .ToArray()
                .Where(u => allMembers.All(d => d.UserId != u.Id));

            var viewModel = new MemberManagementViewModel
            {
                Members = members,
                AddableUsers = allUsers.ToArray(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMembers(string userName, string firstname, string lastname)
        {
            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(firstname) ||
                string.IsNullOrWhiteSpace(lastname))
            {
                ShowErrorMessage("Kein Benutzername, Vor- oder Nachname!");
            }
            else
            {
                try
                {
                    await diverService.AddMembersAsync(diverRepository, userName, firstname, lastname);
                    await context.SaveChangesAsync();
                    ShowSuccessMessage("Mitglied erfolgreich hinzugefügt!");
                }
                catch (Exception ex)
                {                
                    ShowErrorMessage($"Fehler beim Hinzufügen des Mitgliedes {userName}: {ex.UnwindMessage()}");
                }
            }

            return RedirectToAction("MemberManagement");
        }

        [HttpGet]
        public async Task<IActionResult> EditRoles(string userName)
        {
            var member = await diverService.GetMemberAsync(diverRepository, userName);
            if (member == null)
            {
                return BadRequest();
            }

            var assignedRoles = await userManager.GetRolesAsync(member.User);

            return View(new EditRolesViewModel
            {
                Roles = new[] { Rolenames.Tauchbold, Rolenames.Administrator },
                AssignedRoles = assignedRoles,
                Profile = member,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditRoles(string userName, string[] roles)
        {
            var member = await diverService.GetMemberAsync(diverRepository, userName);
            if (member == null)
            {
                return BadRequest();
            }

            await diverService.UpdateRolesAsync(member, roles);
            await context.SaveChangesAsync();

            ShowSuccessMessage($"Rollenzuweisung an {member.Realname} erfolgreich: {string.Join(",", roles)}");
            return RedirectToAction("MemberManagement");
        }

        [HttpGet]
        public async Task<IActionResult> ConfigureRoles()
        {

            await roleManager.CreateAsync(new IdentityRole(Rolenames.Tauchbold));
            await roleManager.CreateAsync(new IdentityRole(Rolenames.Administrator));

            return Ok();
        }

        public async Task<IActionResult> ConfigureUserMarc()
        {

            try
            {
                var userMarc = await context.Users
                                       .Where(u => u.UserName.Equals("marc@marcduerst.com", StringComparison.InvariantCultureIgnoreCase))
                                       .FirstOrDefaultAsync();
                if (userMarc != null)
                {
                    await userManager.AddToRoleAsync(userMarc, Rolenames.Tauchbold);
                    await userManager.AddToRoleAsync(userMarc, Rolenames.Administrator);
                    return Ok();
                }

                return BadRequest("General error!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<MemberViewModel> CreateMemberViewModel(Diver diver)
        {
            var roles = await userManager.GetRolesAsync(diver.User);

            return new MemberViewModel
            {
                Roles = roles,
                Profile = diver,
            };
        }
    }
}
