using System;
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

            return await context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));
        }
    }
}