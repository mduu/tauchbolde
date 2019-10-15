using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.GetUserProfileUseCase
{
    [UsedImplicitly]
    internal class GetUserProfileInteractor : IRequestHandler<GetUserProfile, UseCaseResult>
    {
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly ICurrentUser currentUser;
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly ILogger<GetUserProfileInteractor> logger;

        public GetUserProfileInteractor(
            [NotNull] IDiverRepository diverRepository,
            [NotNull] ICurrentUser currentUser,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] ILogger<GetUserProfileInteractor> logger)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetUserProfile request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var diver = await diverRepository.FindByIdAsync(request.UserId);
            if (diver == null)
            {
                return UseCaseResult.NotFound();
            }
            
            var currentUserDiver = await currentUser.GetCurrentDiverAsync();
            if (currentUserDiver == null)
            {
                logger.LogWarning("Diver record for current user [{id}] not found!", currentUser.Username);
                return UseCaseResult.Fail();
            }
            
            var allowEdit = currentUserDiver?.Id == diver.Id || await currentUser.GetIsAdminAsync();
            var roles = await userManager.GetRolesAsync(diver.User);
            
            request.OutputPort?.Output(new GetUserProfileOutput(
                allowEdit,
                request.UserId,
                roles,
                diver.User.Email,
                diver.AvatarId,
                diver.Realname,
                diver.Firstname,
                diver.Lastname,
                diver.MemberSince,
                diver.Slogan,
                diver.Education,
                diver.Experience,
                diver.MobilePhone,
                diver.WebsiteUrl,
                diver.TwitterHandle,
                diver.FacebookId,
                diver.SkypeId));
            
            return UseCaseResult.Success();
        }
    }
}