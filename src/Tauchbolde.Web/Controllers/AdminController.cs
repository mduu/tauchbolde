using System;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tauchbolde.Common;
using Microsoft.AspNetCore.Identity;

namespace Tauchbolde.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IActionResult> ConfigureRoles()
        {
            await roleManager.CreateAsync(new IdentityRole(Rolenames.Tauchbold));

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
                    return Ok();
                }

                return BadRequest("General error!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
