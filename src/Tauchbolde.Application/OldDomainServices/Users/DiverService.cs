using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Users
{
    internal class DiversService : IDiverService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DiversService"/> class.
        /// </summary>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="userManager">The Identity UserManager to use.</param>
        public DiversService(
            [NotNull] RoleManager<IdentityRole> roleManager,
            [NotNull] UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <inheritdoc/>
        public async Task UpdateRolesAsync(Diver member, ICollection<string> roles)
        {
            if (member == null) { throw new ArgumentNullException(nameof(member)); }

            var allRoles = roleManager.Roles.ToArray();

            foreach (var role in allRoles)
            {
                if (roles.Contains(role.Name))
                {
                    if (!await userManager.IsInRoleAsync(member.User, role.Name))
                    {
                        await userManager.AddToRoleAsync(member.User, role.Name);
                    }
                }
                else
                {
                    if (await userManager.IsInRoleAsync(member.User, role.Name))
                    {
                        await userManager.RemoveFromRoleAsync(member.User, role.Name);
                    }
                }
            }
        }
    }
}
