using System;
using Tauchbolde.Domain.SharedKernel;

namespace Tauchbolde.Domain.Events
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