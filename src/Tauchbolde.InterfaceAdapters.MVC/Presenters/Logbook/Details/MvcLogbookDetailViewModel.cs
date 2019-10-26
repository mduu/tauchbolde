using System;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Logbook.Details
{
    /// <summary>
    /// View model used for rendering Logbook views.
    /// </summary>
    public class MvcLogbookDetailViewModel
    {
        public MvcLogbookDetailViewModel(bool allowEdit,
            Guid id,
            [NotNull] string title,
            bool isFavorite,
            bool isPublished,
            string teaser,
            string text,
            string externalPhotoAlbumUrl,
            string eventTitle,
            string originalAuthorName,
            string originalAuthorEmail,
            string originalAuthorAvatarId,
            string editorName,
            string editorEmail,
            string editorAvatarId,
            string createdAt,
            string modifiedAt,
            string editUrl,
            string unpublishUrl,
            string publishUrl,
            string deleteUrl,
            string eventUrl,
            string teaserImageUrl)
        {
            AllowEdit = allowEdit;
            Id = id;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            IsFavorite = isFavorite;
            IsPublished = isPublished;
            Teaser = teaser;
            Text = text;
            ExternalPhotoAlbumUrl = externalPhotoAlbumUrl;
            EventTitle = eventTitle;
            OriginalAuthorName = originalAuthorName;
            OriginalAuthorEmail = originalAuthorEmail;
            OriginalAuthorAvatarId = originalAuthorAvatarId;
            EditorName = editorName;
            EditorEmail = editorEmail;
            EditorAvatarId = editorAvatarId;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            EditUrl = editUrl;
            UnpublishUrl = unpublishUrl;
            PublishUrl = publishUrl;
            DeleteUrl = deleteUrl;
            EventUrl = eventUrl;
            TeaserImageUrl = teaserImageUrl;
        }

        public bool AllowEdit { get; }
        public Guid Id { get; }
        [NotNull] public string Title { get; }
        public bool IsFavorite { get; }
        public bool IsPublished { get; }
        public string Teaser { get; }
        public string Text { get; }
        public string ExternalPhotoAlbumUrl { get; }
        public string EventTitle { get; }
        
        public string OriginalAuthorName { get; }
        public string OriginalAuthorEmail { get; }
        public string OriginalAuthorAvatarId { get; }
        
        public string EditorName { get; }
        public string EditorEmail { get; }
        public string EditorAvatarId { get; }
        public string CreatedAt { get; }
        public string ModifiedAt { get; }

        public string EditUrl { get; }
        public string UnpublishUrl { get; }
        public string PublishUrl { get; }
        public string DeleteUrl { get; }
        public string EventUrl { get; }
        public string TeaserImageUrl { get; }
    }
}