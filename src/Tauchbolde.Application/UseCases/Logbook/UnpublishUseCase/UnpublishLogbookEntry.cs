using System;
using MediatR;

namespace Tauchbolde.Application.UseCases.Logbook.UnpublishUseCase
{
    public class UnpublishLogbookEntry : IRequest<bool>
    {
        public UnpublishLogbookEntry(Guid logbookEntryId)
        {
            LogbookEntryId = logbookEntryId;
        }

        public Guid LogbookEntryId { get; }
    }
}