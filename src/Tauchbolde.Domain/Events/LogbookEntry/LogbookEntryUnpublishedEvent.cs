using System;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.LogbookEntry
{
    public class LogbookEntryUnpublishedEvent : DomainEventBase
    {
        public Guid LogbookEntryId { get; }

        public LogbookEntryUnpublishedEvent(Guid logbookEntryId)
        {
            LogbookEntryId = logbookEntryId;
        }
    }
}