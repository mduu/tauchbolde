using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.EditUserProfileUseCase
{
    [UsedImplicitly]
    internal class EditUserProfileInteractor : IRequestHandler<EditUserProfile, UseCaseResult>
    {
        [NotNull] private readonly ILogger<EditUserProfileInteractor> logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly ICurrentUser currentUser;

        public EditUserProfileInteractor(
            [NotNull] ILogger<EditUserProfileInteractor> logger,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] EditUserProfile request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Edit user [{DiverId}], [{@Request}]", request.UserId, request);

            var diver = await diverRepository.FindByIdAsync(request.UserId);
            if (diver == null)
            {
                logger.LogWarning("Diver for editing not found. Id [{DiverId}]", request.UserId);
                return UseCaseResult.NotFound();
            }

            var user = await currentUser.GetCurrentDiverAsync();
            var isAdmin = await currentUser.GetIsAdminAsync();
            
            if (diver.Id != user.Id && !isAdmin)
            {
                logger.LogError("Access denied: User [{EditorId}] is not allowed to edit user [{UserId}]!", user.Id, request.UserId);
                return UseCaseResult.AccessDenied();
            }

            diver.Edit(
                user.Id,
                request.Fullname,
                request.Firstname,
                request.Lastname,
                request.Education,
                request.Experience,
                request.Slogan,
                request.MobilePhone,
                request.WebsiteUrl,
                request.FacebookId,
                request.TwitterHandle,
                request.SkypeId);

            await diverRepository.UpdateAsync(diver);
            
            logger.LogError("User [{EditorId}] successfully edited by user [{UserId}]! {@Request}", user.Id, request.UserId, request);

            return UseCaseResult.Success();
        }
    }
}