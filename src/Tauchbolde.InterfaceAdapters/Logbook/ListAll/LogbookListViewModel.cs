using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.Logbook.ListAll
{
    public class LogbookListViewModel
    {
        public LogbookListViewModel( bool allowEdit, [NotNull] IEnumerable<LogbookItemViewModel> logbookItems)
        {
            AllowEdit = allowEdit;
            LogbookItems = logbookItems ?? throw new ArgumentNullException(nameof(logbookItems));
        }
        
        public IEnumerable<LogbookItemViewModel> LogbookItems { get; }
        public bool AllowEdit { get; }
        
        public class LogbookItemViewModel
        {
            public LogbookItemViewModel(
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
                Text = text;
                IsPublished = isPublished;
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