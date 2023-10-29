using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.LogbookEntry
{
    public class LogbookEntryDeletedEvent : DomainEventBase
    {
        public LogbookEntryDeletedEvent(Guid logbookEntryId, string teaserImage, string teaserImageThumb)
        {
            LogbookEntryId = logbookEntryId;
            TeaserImage = teaserImage;
            TeaserImageThumb = teaserImageThumb;
        }

        public Guid LogbookEntryId { get; }
        public string TeaserImage { get; }
        public string TeaserImageThumb { get; }
    }
}