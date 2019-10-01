using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration
{
    [UsedImplicitly]
    internal class GetMassMailDetailsInteractor : IRequestHandler<GetMassMailDetails, UseCaseResult>
    {
        [NotNull] private readonly IDiverRepository diverRepository;

        public GetMassMailDetailsInteractor([NotNull] IDiverRepository diverRepository)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetMassMailDetails request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var members = await diverRepository.GetAllTauchboldeUsersAsync();
            var output = new GetMassMailDetailsOutput(
                members.Select(m =>
                    new GetMassMailDetailsOutput.MailRecipient(
                        m.Fullname,
                        m.User.Email)));

            request.OutputPort?.Output(output);

            return UseCaseResult.Success();
        }
    }
}