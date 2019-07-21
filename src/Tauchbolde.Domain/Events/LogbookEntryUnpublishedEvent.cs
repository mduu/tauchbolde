using System;
using Tauchbolde.Domain.SharedKernel;

namespace Tauchbolde.Domain.Events
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