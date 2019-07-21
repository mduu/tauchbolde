using System;
using Tauchbolde.Domain.SharedKernel;

namespace Tauchbolde.Domain.Events.LogbookEntry
{
    public class LogbookEntryPublishedEvent : DomainEventBase
    {
        public Guid LogbookEntryId { get; }

        public LogbookEntryPublishedEvent(Guid logbookEntryId)
        {
            LogbookEntryId = logbookEntryId;
        }
    }
}