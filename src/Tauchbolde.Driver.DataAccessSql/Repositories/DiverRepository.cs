using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;
using Rolenames = Tauchbolde.Domain.Types.Rolenames;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    internal class DiverRepository : RepositoryBase<Diver, DiverData>, IDiverRepository
    {
        private readonly UserManager<IdentityUser> userManager;

        public DiverRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager) : base(context)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <inheritdoc />
        public async Task<Diver> FindByUserNameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            return
                (await Context.Diver
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(u =>
                        u.User.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase)))
                .MapTo();
        }

        /// <inheritdoc />
        public override async Task<Diver> FindByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid.Empty not allowed!", nameof(id));
            }

            return
                (await Context.Diver
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(u => u.Id == id))
                .MapTo();
        }

        /// <inheritdoc />
        public async Task<ICollection<Diver>> GetAllTauchboldeUsersAsync(bool includingAdmins = false)
        {
            var tauchboldeRole = await Context.Roles.FirstOrDefaultAsync(r => r.Name == Rolenames.Tauchbold);

            var usersIds = (await userManager.GetUsersInRoleAsync(Rolenames.Tauchbold))
                .Select(u => u.Id)
                .ToList();

            if (includingAdmins)
            {
                var admins = (await userManager.GetUsersInRoleAsync(Rolenames.Administrator))
                    .Select(u => u.Id)
                    .ToList();

                usersIds = usersIds.Union(admins).Distinct().ToList();
            }

            var divers =
                (await Context.Diver
                    .Include(u => u.User)
                    .OrderBy(d => d.Firstname)
                    .ThenBy(d => d.Lastname)
                    .ToListAsync())
                .Select(d => d.MapTo())
                .ToList();

            return divers
                .Where(u => usersIds.Contains(u.UserId))
                .ToArray();
        }

        /// <inheritdoc />
        public async Task<ICollection<Diver>> GetAllDiversAsync() =>
            (await Context.Diver
                .Include(u => u.User)
                .OrderBy(d => d.Firstname)
                .ThenBy(d => d.Lastname)
                .ToListAsync())
            .Select(d => d.MapTo())
            .ToList();
   
        protected override Diver MapTo(DiverData dataEntity) => dataEntity.MapTo();
        protected override DiverData MapTo(Diver domainEntity) => domainEntity.MapTo();
    }
}