using System;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tauchbolde.Common;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.EventViewModels;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace Tauchbolde.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RequireAdministrator)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IDiverRepository diverRepository;

        public AdminController(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            IDiverRepository diverRepository)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MemberManagement()
        {
            var profiles = (await diverRepository.GetAllTauchboldeUsersAsync()).ToArray();

            var members = new List<MemberViewModel>();
            foreach (var member in profiles)
            {
                members.Add(await CreateMemberViewModel(member));
            }

            var viewModel = new MemberManagementViewModel
            {
                Members = members,
            };

            return View(viewModel);
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
            return new MemberViewModel
            {
                Roles = await userManager.GetRolesAsync(diver.User),
                Profile = diver,
            };
        }
    }
}
