using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.SummaryListUseCase
{
    [UsedImplicitly]
    internal class SummaryListLogbookEntriesHandler : IRequestHandler<SummaryListLogbookEntries, UseCaseResult<IEnumerable<LogbookEntry>>>
    {
        private readonly ILogbookEntryRepository logbookEntryRepository;

        public SummaryListLogbookEntriesHandler([NotNull] ILogbookEntryRepository logbookEntryRepository)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
        }
        
        public async Task<UseCaseResult<IEnumerable<LogbookEntry>>> Handle([NotNull] SummaryListLogbookEntries request, CancellationToken cancellationToken)
        {
            var allEntries = await logbookEntryRepository.GetAllEntriesAsync(false);
            
            return UseCaseResult<IEnumerable<LogbookEntry>>.Success(allEntries);
        }
    }
}