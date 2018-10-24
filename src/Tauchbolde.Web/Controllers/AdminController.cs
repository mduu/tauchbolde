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

namespace Tauchbolde.Web.Controllers
{
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
        
        public async Task<IActionResult> ShowMyRoles()
        {
            var currentDiver = await diverRepository.FindByUserNameAsync(User.Identity.Name);
            if (currentDiver == null)
            {
                return Json(new { error = $"Diver not found with username {User.Identity.Name}."});
            }

            var roles = await userManager.GetRolesAsync(currentDiver.User);

            return Json(new {
                UserIdentityName = User.Identity.Name,
                Roles = roles.ToArray()
             });
        }
    }
}
