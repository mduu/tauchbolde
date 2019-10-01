using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    [UsedImplicitly]
    internal class ListAllLogbookEntriesInteractor : IRequestHandler<ListAllLogbookEntries, UseCaseResult>
    {
        [NotNull] private readonly ILogbookEntryRepository logbookEntryRepository;
        [NotNull] private readonly ICurrentUser currentUser;

        public ListAllLogbookEntriesInteractor(
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] ICurrentUser currentUser)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] ListAllLogbookEntries request, CancellationToken cancellationToken)
        {
            var canEdit = await currentUser.GetIsTauchboldOrAdminAsync();
            var allEntries = await logbookEntryRepository.GetAllEntriesAsync(canEdit);
            var output = new ListAllLogbookEntriesOutputPort(allEntries.Select(l => 
                new ListAllLogbookEntriesOutputPort.LogbookItem(
                    l.Id, l.Title, l.TeaserText, l.TeaserImageThumb, l.IsPublished, l.Text)),
                canEdit);
            
            request.OutputPort.Output(output);
            
            return UseCaseResult.Success();
        }
    }
}