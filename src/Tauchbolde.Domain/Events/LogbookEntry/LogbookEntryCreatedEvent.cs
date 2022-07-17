using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.LogbookEntry
{
    public class LogbookEntryCreatedEvent : DomainEventBase
    {
        public LogbookEntryCreatedEvent(Guid logbookEntryId, string logbookEntryTitle)
        {
            LogbookEntryId = logbookEntryId;
            LogbookEntryTitle = logbookEntryTitle;
        }

        public Guid LogbookEntryId { get; }
        public string LogbookEntryTitle { get; }
    }
}