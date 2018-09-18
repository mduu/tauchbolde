using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<ApplicationUser> FindByUserNameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) { throw new ArgumentNullException(nameof(username)); }

            return await Context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));
        }

        /// <inheritdoc />
        public async Task<ICollection<ApplicationUser>> GetAllTauchboldeUsersAsync()
        {
            var tauchboldeRole = await Context.Roles.FirstOrDefaultAsync(r => r.Name == Rolenames.Tauchbold);

            return await Context.Users
                .Include(u => u.AdditionalUserInfos)
                .Include(u => u.Roles)
                .Where(u =>
                    !u.LockoutEnabled && u.Roles.Any(r => r.RoleId == tauchboldeRole.Id))
                .ToListAsync();
        }
    }
}