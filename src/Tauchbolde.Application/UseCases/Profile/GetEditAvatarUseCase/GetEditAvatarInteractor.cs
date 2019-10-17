using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.GetEditAvatarUseCase
{
    [UsedImplicitly]
    internal class GetEditAvatarInteractor : IRequestHandler<GetEditAvatar, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly ICurrentUser currentUser;
        
        public GetEditAvatarInteractor(
            [NotNull] ILogger<GetEditAvatarInteractor> logger,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] GetEditAvatar request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var diver = await diverRepository.FindByIdAsync(request.UserId);
            if (diver == null)
            {
                return UseCaseResult.NotFound();
            }
            
            var currentUserDiver = await currentUser.GetCurrentDiverAsync();
            var isAdmin = await currentUser.GetIsAdminAsync();

            if (!isAdmin && diver.Id != currentUserDiver.Id)
            {
                return UseCaseResult.AccessDenied();
            }
            
            request.OutputPort?.Output(new GetEditAvatarOutput(diver.Id, diver.Realname));
            
            return UseCaseResult.Success();
        }
    }
}