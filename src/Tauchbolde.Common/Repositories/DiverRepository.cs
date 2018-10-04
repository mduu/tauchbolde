using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    public class DiverRepository : RepositoryBase<Diver>, IDiverRepository
    {
        public DiverRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<Diver> FindByUserNameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) { throw new ArgumentNullException(nameof(username)); }

            return await Context.Diver
                                .Include(d => d.User)
                                .FirstOrDefaultAsync(u => u.User.UserName.Equals(username));
        }

        /// <inheritdoc />
        public async Task<ICollection<Diver>> GetAllTauchboldeUsersAsync()
        {
            var tauchboldeRole = await Context.Roles.FirstOrDefaultAsync(r => r.Name == Rolenames.Tauchbold);

            var usersIds = await Context.UserRoles
                                        .Where(u => u.RoleId == tauchboldeRole.Id)
                                        .Select(u => u.UserId).ToListAsync();

            return await Context.Diver
                                .Include(u => u.User)
                                .Where(u => !u.User.LockoutEnabled && usersIds.Any(i => i == u.UserId))
                                .ToListAsync();
        }
    }
}