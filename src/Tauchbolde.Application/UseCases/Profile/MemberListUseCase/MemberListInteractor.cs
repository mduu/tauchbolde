using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.MemberListUseCase
{
    [UsedImplicitly]
    internal class MemberListInteractor : IRequestHandler<MemberList, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly ICurrentUser currentUser;

        public MemberListInteractor(
            [NotNull] ILogger<MemberListInteractor> logger,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<UseCaseResult> Handle([NotNull] MemberList request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var allowSeeDetails = await currentUser.GetIsTauchboldOrAdminAsync();
            var divers = await diverRepository.GetAllTauchboldeUsersAsync();

            request.OutputPort?.Output(new MemberListOutput(
                allowSeeDetails,
                divers
                    .Where(d => d.MemberSince.HasValue)
                    .Select(d => new MemberListOutput.Member(
                        d.Id,
                        d.User.Email,
                        d.AvatarId,
                        d.Realname,
                        d.MemberSince.Value,
                        d.Education,
                        d.Experience,
                        d.Slogan,
                        d.WebsiteUrl,
                        d.TwitterHandle,
                        d.FacebookId))));

            return UseCaseResult.Success();
        }
    }
}