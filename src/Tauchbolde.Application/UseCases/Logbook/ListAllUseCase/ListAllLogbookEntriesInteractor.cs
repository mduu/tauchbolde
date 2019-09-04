using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    [UsedImplicitly]
    internal class ListAllLogbookEntriesInteractor : IRequestHandler<ListAllLogbookEntries, UseCaseResult>
    {
        private readonly ILogbookEntryRepository logbookEntryRepository;

        public ListAllLogbookEntriesInteractor([NotNull] ILogbookEntryRepository logbookEntryRepository)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] ListAllLogbookEntries request, CancellationToken cancellationToken)
        {
            var allEntries = await logbookEntryRepository.GetAllEntriesAsync(request.IncludeUnpublished);
            
            var output = new ListAllLogbookEntriesOutputPort(allEntries.Select(l => 
                new ListAllLogbookEntriesOutputPort.LogbookItem(
                    l.Id, l.Title, l.TeaserText, l.TeaserImageThumb, l.IsPublished, l.Text)));
            
            request.OutputPort.Output(output);
            
            return UseCaseResult.Success();
        }
    }
}