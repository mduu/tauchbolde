using System;

namespace Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase
{
    public class GetLogbookEntryDetailOutput
    {
        public GetLogbookEntryDetailOutput(
            Guid logbookEntryId,
            bool allowEdit,
            string title,
            string teaser,
            string text,
            string externalPhotoAlbumUrl,
            string teaserImageIdentifier,
            string teaserThumbImageIdentifier,
            string eventName,
            Guid? eventId,
            bool isFavorite,
            bool isPublished,
            string originalAuthorName,
            string originalAuthorEmail,
            string originalAuthorAvatarId,
            string editorName,
            string editorEmail,
            string editorAvatarId,
            DateTime createdAt,
            DateTime? modifiedAt)
        {
            LogbookEntryId = logbookEntryId;
            AllowEdit = allowEdit;
            Title = title;
            Teaser = teaser;
            Text = text;
            ExternalPhotoAlbumUrl = externalPhotoAlbumUrl;
            TeaserImageIdentifier = teaserImageIdentifier;
            TeaserThumbImageIdentifier = teaserThumbImageIdentifier;
            EventName = eventName;
            EventId = eventId;
            IsFavorite = isFavorite;
            IsPublished = isPublished;
            OriginalAuthorName = originalAuthorName;
            OriginalAuthorEmail = originalAuthorEmail;
            OriginalAuthorAvatarId = originalAuthorAvatarId;
            EditorName = editorName;
            EditorEmail = editorEmail;
            EditorAvatarId = editorAvatarId;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
        }

        public Guid LogbookEntryId { get; }
        public bool AllowEdit { get; }
        public string Title { get; }
        public string Teaser { get; }
        public string Text { get; }
        public string ExternalPhotoAlbumUrl { get; }
        public string TeaserImageIdentifier { get; }
        public string TeaserThumbImageIdentifier { get; }
        public string EventName { get; }
        public Guid? EventId { get; }
        public bool IsFavorite { get; }
        public bool IsPublished { get; }
        public string OriginalAuthorName { get; }
        public string OriginalAuthorEmail { get; }
        public string OriginalAuthorAvatarId { get; }
        public string EditorName { get; }
        public string EditorEmail { get; }
        public string EditorAvatarId { get; }
        public DateTime CreatedAt { get; }
        public DateTime? ModifiedAt { get; }
    }
}