using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    public class ListAllLogbookEntriesOutputPort
    {
        public ListAllLogbookEntriesOutputPort([NotNull] IEnumerable<LogbookItem> logbookItems)
        {
            LogbookItems = logbookItems ?? throw new ArgumentNullException(nameof(logbookItems));
        }

        public IEnumerable<LogbookItem> LogbookItems { get; }
        
        public class LogbookItem
        {
            public LogbookItem(
                Guid logbookEntryId,
                [NotNull] string title,
                [CanBeNull] string teaserText,
                [CanBeNull] string teaserImageUrl,
                bool isPublished,
                [CanBeNull] string text)
            {
                LogbookEntryId = logbookEntryId;
                Title = title ?? throw new ArgumentNullException(nameof(title));
                TeaserText = teaserText;
                TeaserImageUrl = teaserImageUrl;
                IsPublished = isPublished;
                Text = text;
            }

            public Guid LogbookEntryId { get; }
            [NotNull] public string Title { get; }
            [CanBeNull] public string TeaserText { get; }
            [CanBeNull] public string TeaserImageUrl { get; }
            public bool IsPublished { get; }
            [CanBeNull] public string Text { get; }
        }
    }
}