using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.Extensions.FileProviders;

namespace Tauchbolde.Common.DomainServices
{
    public class DiversService : IDiverService
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DiversService"/> class.
        /// </summary>
        /// <param name="applicationDbContext">Application db context.</param>
        /// <param name="userManager">The Identity UserManager to use.</param>
        public DiversService(
            ApplicationDbContext applicationDbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            context = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <inheritdoc/>
        public async Task<ICollection<Diver>> GetMembersAsync(IDiverRepository diverRepository)
        {
            if (diverRepository == null) { throw new ArgumentNullException(nameof(diverRepository)); }

            return await diverRepository.GetAllTauchboldeUsersAsync();
        }

        /// <inheritdoc/>
        public async Task<Diver> GetMemberAsync(IDiverRepository diverRepository, string userName)
        {
            if (diverRepository == null) { throw new ArgumentNullException(nameof(diverRepository)); }

            return await diverRepository.FindByUserNameAsync(userName);
        }

        public async Task UpdateUserProfil(IDiverRepository diverRepository, Diver profile)
        {
            if (diverRepository == null) { throw new ArgumentNullException(nameof(diverRepository)); }
            if (profile == null) { throw new ArgumentNullException(nameof(profile)); }

            var diver = await diverRepository.FindByUserNameAsync(profile.User.UserName);
            if (diver == null)
            {
                throw new InvalidOperationException("Profile (Diver) nicht gefunden!");
            }

            diver.Fullname = profile.Fullname;
            diver.Firstname = profile.Firstname;
            diver.Lastname = profile.Lastname;
            diver.Education = profile.Education;
            diver.Experience = profile.Experience;
            diver.Slogan = profile.Slogan;
            diver.WebsiteUrl = profile.WebsiteUrl;
            diver.TwitterHandle = profile.TwitterHandle;
            diver.FacebookId = profile.FacebookId;
            diver.SkypeId = profile.SkypeId;
            diver.MobilePhone = profile.MobilePhone;

            diverRepository.Update(diver);
        }

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

        public async Task<string> AddMembersAsync(IDiverRepository diverRepository, string userName, string firstname, string lastname)
        {
            if (diverRepository == null) { throw new ArgumentNullException(nameof(diverRepository)); }
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrWhiteSpace(firstname)) throw new ArgumentNullException(nameof(firstname));
            if (string.IsNullOrWhiteSpace(lastname)) throw new ArgumentNullException(nameof(lastname));

            string warningMessage = "";

            var user = await userManager.FindByNameAsync(userName);

            var diver = new Diver
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Firstname = firstname,
                Lastname = lastname,
                Fullname = $"{firstname} {lastname}",
                MobilePhone = user.PhoneNumber,
            };

            await userManager.AddToRoleAsync(user, Rolenames.Tauchbold);

            await diverRepository.InsertAsync(diver);

            if (user.LockoutEnabled)
            {
                warningMessage += "User ist noch gesperrt (LockoutEnabled). ";
            }

            if (!user.EmailConfirmed)
            {
                warningMessage += "User hat seine Emailadresse noch nicht bestätigt (EmailConfirmed=false). ";
            }

            return warningMessage.Trim();
        }
    }
}
