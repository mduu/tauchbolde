using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.UseCases.Administration.AddMemberUseCase;
using Tauchbolde.Driver.DataAccessSql;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;
using Tauchbolde.Web.Models.AdminViewModels;
using Tauchbolde.Web.Core;

namespace Tauchbolde.Web.Controllers
{
    // TODO Change the implementation of the actions to use use-cases instead of direct access
    [Authorize(Policy = PolicyNames.RequireAdministrator)]
    public class AdminController : AppControllerBase
    {
        [NotNull] private readonly ApplicationDbContext context;
        [NotNull] private readonly RoleManager<IdentityRole> roleManager;
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly IDiverService diverService;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IMediator mediator;

        public AdminController(
            [NotNull] ApplicationDbContext context,
            [NotNull] RoleManager<IdentityRole> roleManager,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] IDiverService diverService,
            [NotNull] IDiverRepository diverRepository, 
            [NotNull] IMediator mediator)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> MemberManagement()
        {
            var profiles = (await diverRepository.GetAllDiversAsync()).ToArray();

            var members = new List<MemberViewModel>();
            foreach (var member in profiles)
            {
                members.Add(await CreateMemberViewModel(member));
            }

            var allMembers = await diverRepository.GetAllTauchboldeUsersAsync();
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
            var useCaseResult = await mediator.Send(new AddMember(userName, firstname, lastname));

            var msgMap = new Dictionary<ResultCategory, Action>
            {
                { ResultCategory.Success, () => ShowSuccessMessage("Mitglied erfolgreich hinzugefügt!") },
                { ResultCategory.NotFound, () => ShowErrorMessage("Kein registrierter Benutzer mit Benutzername '{username}' gefunden!") },
                { ResultCategory.AccessDenied, () => ShowErrorMessage("Sie haben keine Berechtigung um neue Mitglieder hinzu zu fügen!")},
            };
            msgMap[useCaseResult.ResultCategory]();

            if (!string.IsNullOrWhiteSpace(useCaseResult.Payload))
            {
                ShowWarningMessage(useCaseResult.Payload);
            }

            return RedirectToAction(nameof(MemberManagement));
        }

        [HttpGet]
        public async Task<IActionResult> EditRoles(string userName)
        {
            var member = await diverRepository.FindByUserNameAsync(userName);
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
            var member = await diverRepository.FindByUserNameAsync(userName);
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
