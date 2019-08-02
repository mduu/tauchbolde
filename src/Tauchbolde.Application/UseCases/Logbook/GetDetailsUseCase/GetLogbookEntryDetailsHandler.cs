using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase
{
    [UsedImplicitly]
    public class GetLogbookEntryDetailsHandler : IRequestHandler<GetLogbookEntryDetails, UseCaseResult<LogbookEntry>>
    {
        private readonly ILogger<GetLogbookEntryDetailsHandler> logger;
        [NotNull] private readonly ILogbookEntryRepository repository;

        public GetLogbookEntryDetailsHandler(
            [NotNull] ILogger<GetLogbookEntryDetailsHandler> logger,
            [NotNull] ILogbookEntryRepository repository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<UseCaseResult<LogbookEntry>> Handle([NotNull] GetLogbookEntryDetails request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            logger.LogDebug("Get details for LogbookEntry [{id}]", request.LogbookEntryId);

            var details = await repository.FindByIdAsync(request.LogbookEntryId);
            if (details == null)
            {
                return UseCaseResult<LogbookEntry>.NotFound(
                    nameof(LogbookEntry),
                    nameof(GetLogbookEntryDetails.LogbookEntryId),
                    request.LogbookEntryId);
            }

            logger.LogDebug("Got details for LogbookEntry [{id}] successful", request.LogbookEntryId);

            return UseCaseResult<LogbookEntry>.Success(details);
        }
    }
}