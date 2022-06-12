using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services;
using Tauchbolde.Application.Services.Avatars;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.EditAvatarUseCase
{
    [UsedImplicitly]
    internal class EditAvatarInteractor : IRequestHandler<EditAvatar, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IAvatarStore avatarStore;
        [NotNull] private readonly IMimeMapping mimeMapping;
        [NotNull] private readonly ICurrentUser currentUser;
        
        public EditAvatarInteractor(
            [NotNull] ILogger<EditAvatarInteractor> logger,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IAvatarStore avatarStore,
            [NotNull] IMimeMapping mimeMapping,
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.avatarStore = avatarStore ?? throw new ArgumentNullException(nameof(avatarStore));
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] EditAvatar request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Changing avatar for user [{Id}], {Filename}, {ContentType}", request.UserId, request.Avatar.Filename, request.Avatar.ContentType);

            var diver = await diverRepository.FindByIdAsync(request.UserId);
            if (diver == null)
            {
                logger.LogError("Diver with Id [{id}] not found!", request.UserId);
                return UseCaseResult.NotFound();
            }

            if (!await currentUser.GetIsDiverOrAdmin(request.UserId))
            {
                logger.LogError("Current user [{CurrentUser}] not allowed to change avatar of user [{User}]", currentUser.Username, diver.Realname);
                return UseCaseResult.AccessDenied();
            }

            var newAvatarId = await avatarStore.StoreAvatarAsync(
                diver.Firstname,
                diver.AvatarId,
                GetAvatarFileExt(request),
                request.Avatar.Content);
            
            diver.ChangeAvatar(newAvatarId);
            
            await diverRepository.UpdateAsync(diver);
            
            logger.LogInformation("Avatar of user [{UserId}] change by user [{CurrentUser}]", diver.Realname, currentUser.Username);
            return UseCaseResult.Success();
        }

        private string GetAvatarFileExt(EditAvatar request) => 
            mimeMapping.GetFileExtensionMapping(request.Avatar.ContentType) ?? Path.GetExtension(request.Avatar.Filename);
    }
}