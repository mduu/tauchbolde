using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventNewDetailsUseCase
{
    [UsedImplicitly]
    public class GetEventNewDetailsInteractor : IRequestHandler<GetEventNewDetails, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IClock clock;

        public GetEventNewDetailsInteractor(
            [NotNull] ILogger<GetEventNewDetailsInteractor> logger,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IClock clock)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetEventNewDetails request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var currentDiver = await diverRepository.FindByUserNameAsync(request.CurrentUserName);
            if (currentDiver == null)
            {
                logger.LogError("No diver for current user [{currentUserName}] found!", request.CurrentUserName);
                return UseCaseResult.NotFound();
            }
            
            request.OutputPort?.Output(
                new GetEventNewOutput(
                    currentDiver.Fullname,
                    currentDiver.User.Email,
                    currentDiver.AvatarId,
                    clock.Now().Date.AddDays(1).AddHours(19),
                    null
                ));
            
            return UseCaseResult.Success();
        }
    }
}