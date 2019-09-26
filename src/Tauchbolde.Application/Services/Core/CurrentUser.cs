using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.Services.Core
{
    [UsedImplicitly]
    internal class CurrentUser : ICurrentUser
    {
        [NotNull] private readonly ICurrentUserInformation currentUserInformation;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly UserManager<IdentityUser> userManager;

        public CurrentUser(
            [NotNull] ICurrentUserInformation currentUserInformation,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] UserManager<IdentityUser> userManager)
        {
            this.currentUserInformation = currentUserInformation ?? throw new ArgumentNullException(nameof(currentUserInformation));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public string Username => currentUserInformation.UserName;
        
        public async Task<bool> GetIsAdminAsync()
        {
            var diver = await GetCurrentDiverAsync();
            return diver != null && await userManager.IsInRoleAsync(diver.User, Rolenames.Administrator);
        }

        public async Task<bool> GetIsTauchboldAsync()
        {
            var diver = await GetCurrentDiverAsync();
            return diver != null && await userManager.IsInRoleAsync(diver.User, Rolenames.Tauchbold);
        }

        public async Task<Diver> GetCurrentDiverAsync()
        {
            if (string.IsNullOrWhiteSpace(currentUserInformation.UserName))
            {
                return null;
            }

            return await diverRepository.FindByUserNameAsync(currentUserInformation.UserName);
        }
        
        public async Task<bool> GetIsTauchboldOrAdminAsync() => await GetIsTauchboldAsync() || await GetIsAdminAsync();

    }
}