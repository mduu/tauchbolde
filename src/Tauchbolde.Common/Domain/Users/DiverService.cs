using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Common.Domain.Repositories;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain.Users
{
    internal class DiversService : IDiverService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly IDiverRepository diverRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DiversService"/> class.
        /// </summary>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="userManager">The Identity UserManager to use.</param>
        /// <param name="diverRepository">The <see cref="IDiverRepository"/> to use.</param>
        public DiversService(
            [NotNull] RoleManager<IdentityRole> roleManager,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] IDiverRepository diverRepository)
        {
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }
        
        /// <inheritdoc />
        public async Task<ICollection<Diver>> GetAllRegisteredDiversAsync()
            => await diverRepository.GetAllDiversAsync();

        /// <inheritdoc />
        public async Task<Diver> FindByUserNameAsync(string username) => 
            await diverRepository.FindByUserNameAsync(username);

        /// <inheritdoc/>
        public async Task<ICollection<Diver>> GetMembersAsync() 
            => await diverRepository.GetAllTauchboldeUsersAsync();

        /// <inheritdoc/>
        public async Task<Diver> GetMemberAsync(string userName)
            => await diverRepository.FindByUserNameAsync(userName);

        /// <inheritdoc/>
        public async Task<Diver> GetMemberAsync(Guid diverId)
        {
            if (diverId == Guid.Empty) { throw new ArgumentException("Guid.Empty now allowed!", nameof(diverId)); }
        
            return await diverRepository.FindByIdAsync(diverId);            
        }

        /// <inheritdoc/>
        public async Task UpdateUserProfileAsync(Diver profile)
        {
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

        /// <inheritdoc/>
        public async Task<string> AddMembersAsync(string userName, string firstname, string lastname)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrWhiteSpace(firstname)) throw new ArgumentNullException(nameof(firstname));
            if (string.IsNullOrWhiteSpace(lastname)) throw new ArgumentNullException(nameof(lastname));

            var warningMessage = "";
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

            var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, emailConfirmToken);
            
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
