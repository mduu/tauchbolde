using System;
using JetBrains.Annotations;
using Tauchbolde.Common;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class LogbookCardViewModel
    {
        public LogbookCardViewModel([NotNull] LogbookEntry logbookEntry, bool allowEdit)
        {
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));
            
            AllowEdit = allowEdit;
            Title = logbookEntry.Title;
            TeaserText = !string.IsNullOrWhiteSpace(logbookEntry.TeaserText)
                ? logbookEntry.TeaserText
                : logbookEntry.Text.Substring(0, 250) + "...";
            OriginalAuthorName = logbookEntry.OriginalAuthor?.Realname ?? "";
            EditorAuthorName = logbookEntry.EditorAuthor?.Realname;
            CreatedAt = logbookEntry.CreatedAt.ToStringSwissDateTime();
            ModifiedAt = logbookEntry.ModifiedAt.ToStringSwissDateTime();
            LogbookEntryId = logbookEntry.Id;
        }

        public Guid LogbookEntryId { get; set; }
        public string Title { get; set; }
        public string OriginalAuthorName { get; set; }
        public string EditorAuthorName { get; set; }
        public string TeaserText { get; set; }
        public string CreatedAt { get; set; }
        public string ModifiedAt { get; set; }
        public bool AllowEdit { get; }
    }
}