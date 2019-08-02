using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    [UsedImplicitly]
    internal class ListAllLogbookEntriesHandler : IRequestHandler<ListAllLogbookEntries, UseCaseResult<IEnumerable<LogbookEntry>>>
    {
        private readonly ILogbookEntryRepository logbookEntryRepository;

        public ListAllLogbookEntriesHandler([NotNull] ILogbookEntryRepository logbookEntryRepository)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
        }
        
        public async Task<UseCaseResult<IEnumerable<LogbookEntry>>> Handle([NotNull] ListAllLogbookEntries request, CancellationToken cancellationToken)
        {
            var allEntries = await logbookEntryRepository.GetAllEntriesAsync(request.IncludeUnpublished);
            
            return UseCaseResult<IEnumerable<LogbookEntry>>.Success(allEntries);
        }
    }
}