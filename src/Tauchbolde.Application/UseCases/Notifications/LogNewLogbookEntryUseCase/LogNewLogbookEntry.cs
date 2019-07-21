using System;
using JetBrains.Annotations;
using MediatR;

namespace Tauchbolde.Application.UseCases.Notifications.LogNewLogbookEntryUseCase
{
    public class LogNewLogbookEntry : IRequest
    {
        private LogNewLogbookEntry(
            Guid logbookEntryId,
            [NotNull] string logbookEntryTitle,
            [NotNull] string authorName)
        {
            LogbookEntryId = logbookEntryId;
            LogbookEntryTitle = logbookEntryTitle ?? throw new ArgumentNullException(nameof(logbookEntryTitle));
            AuthorName = authorName ?? throw new ArgumentNullException(nameof(authorName));
        }

        public Guid LogbookEntryId { get; }
        public string LogbookEntryTitle { get; }
        public string AuthorName { get; }
    }
}