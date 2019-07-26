using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.DeleteUseCase
{
    [UsedImplicitly]
    public class DeleteLogbookEntryHandler : IRequestHandler<DeleteLogbookEntry, UseCaseResult>
    {
        private readonly ILogger<DeleteLogbookEntryHandler> logger;
        private readonly ILogbookEntryRepository repository;

        public DeleteLogbookEntryHandler(
            [NotNull] ILogger<DeleteLogbookEntryHandler> logger,
            [NotNull] ILogbookEntryRepository repository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] DeleteLogbookEntry request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            logger.LogInformation("Deleting LogbookEntry with Id [{id}]", request.LogbookEntryId);

            var existingLogbookEntry = await repository.FindByIdAsync(request.LogbookEntryId);
            if (existingLogbookEntry == null)
            {
                return UseCaseResult.Fail(new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.LogbookEntryId), $"Logbookeintrag [{request.LogbookEntryId}] nicht gefunden!")
                });
            }

            await repository.DeleteAsync(existingLogbookEntry);

            logger.LogInformation("Deleted LogbookEntry with Id [{id}] successfully", request.LogbookEntryId);
            return UseCaseResult.Success();
        }
    }
}