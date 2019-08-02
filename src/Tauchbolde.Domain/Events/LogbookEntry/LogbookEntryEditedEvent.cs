using System;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.LogbookEntry
{
    public class LogbookEntryEditedEvent : DomainEventBase
    {
        public LogbookEntryEditedEvent(Guid logbookEntryId, string title)
        {
            LogbookEntryId = logbookEntryId;
            Title = title;
        }

        public Guid LogbookEntryId { get; }
        public string Title { get;}
    }
}