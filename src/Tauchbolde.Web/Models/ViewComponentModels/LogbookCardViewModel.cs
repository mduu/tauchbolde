using System;
using JetBrains.Annotations;
using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class LogbookCardViewModel
    {
        public LogbookCardViewModel(
            [NotNull] LogbookEntry logbookEntry)
        {
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));
            Title = logbookEntry.Title;
            TeaserText = GetTeaserText(logbookEntry);
            LogbookEntryId = logbookEntry.Id;
            TeaserImageUrl = logbookEntry.TeaserImageThumb;
            IsPublished = logbookEntry.IsPublished;
        }

        public Guid LogbookEntryId { get; }
        public string Title { get; }
        public string TeaserText { get; }
        public string TeaserImageUrl { get; }
        public bool IsPublished { get; }

        private static string GetTeaserText(LogbookEntry logbookEntry)
        {
            if (!string.IsNullOrWhiteSpace(logbookEntry.TeaserText))
            {
                return logbookEntry.TeaserText;
            }

            const int teaserLength = 250;
            if (!string.IsNullOrWhiteSpace(logbookEntry.Text) &&
                logbookEntry.Text.Length > teaserLength)
            {
                return logbookEntry.Text.Substring(0, teaserLength) + "...";
            }

            return logbookEntry.Text;
        }
    }
}