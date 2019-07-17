using System;
using MediatR;

namespace Tauchbolde.UseCases.Logbook.PublishUseCase
{
    public class PublishLogbookEntry : IRequest<bool>
    {
        public PublishLogbookEntry(Guid logbookEntryId)
        {
            LogbookEntryId = logbookEntryId;
        }

        public Guid LogbookEntryId { get; }
    }
}