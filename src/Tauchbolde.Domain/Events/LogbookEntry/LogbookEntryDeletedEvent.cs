using System;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.LogbookEntry
{
    public class LogbookEntryDeletedEvent : DomainEventBase
    {
        public LogbookEntryDeletedEvent(Guid logbookEntryId)
        {
            LogbookEntryId = logbookEntryId;
        }

        public Guid LogbookEntryId { get; }
    }
}