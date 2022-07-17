using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.DeleteUseCase
{
    public class DeleteLogbookEntry : IRequest<UseCaseResult>
    {
        public DeleteLogbookEntry(Guid logbookEntryId)
        {
            LogbookEntryId = logbookEntryId;
        }

        public Guid LogbookEntryId { get; }
    }
}