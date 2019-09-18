using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.Services.Core
{
    [UsedImplicitly]
    internal class CurrentUser : ICurrentUser
    {
        [NotNull] private readonly ICurrentUserInformation currentUserInformation;
        [NotNull] private readonly IDiverRepository diverRepository;

        public CurrentUser(
            [NotNull] ICurrentUserInformation currentUserInformation,
            [NotNull] IDiverRepository diverRepository)
        {
            this.currentUserInformation = currentUserInformation ?? throw new ArgumentNullException(nameof(currentUserInformation));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }

        public async Task<Diver> GetCurrentDiver()
        {
            if (string.IsNullOrWhiteSpace(currentUserInformation.UserName))
            {
                return null;
            }

            return await diverRepository.FindByUserNameAsync(currentUserInformation.UserName);
        }
    }
}