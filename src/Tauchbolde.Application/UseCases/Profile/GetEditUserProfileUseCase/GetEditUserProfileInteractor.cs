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

namespace Tauchbolde.Application.UseCases.Profile.GetEditUserProfileUseCase
{
    [UsedImplicitly]
    public class GetEditUserProfileInteractor : IRequestHandler<GetEditUserProfile, UseCaseResult>
    {
        [NotNull] private readonly ILogger<GetEditUserProfileInteractor> logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly ICurrentUser currentUser;

        public GetEditUserProfileInteractor(
            [NotNull] ILogger<GetEditUserProfileInteractor> logger,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetEditUserProfile request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var diver = await diverRepository.FindByIdAsync(request.UserId);
            if (diver == null)
            {
                return UseCaseResult.NotFound();
            }

            request.OutputPort?.Output(
                new GetEditUserProfileOutput(
                    request.UserId,
                    diver.Fullname,
                    diver.Firstname,
                    diver.Lastname,
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