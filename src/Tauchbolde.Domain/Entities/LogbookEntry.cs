using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Tauchbolde.Domain.Events.LogbookEntry;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    [UsedImplicitly]
    public class LogbookEntry : EntityBase
    {
        public LogbookEntry(
            [NotNull] string title,
            [NotNull] string teaser,
            [NotNull] string text,
            bool isFavorite,
            Guid authorDiverId,
            [CanBeNull] string externalPhotoAlbumUrl,
            [CanBeNull] Guid? relatedEventId,
            [CanBeNull] PhotoAndThumbnailIdentifiers photoIdentifiers)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
            if (string.IsNullOrWhiteSpace(teaser))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(teaser));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

            Id = Guid.NewGuid();
            Title = title;
            TeaserText = teaser;
            Text = text;
            IsFavorite = isFavorite;
            IsPublished = false;
            OriginalAuthorId = authorDiverId;
            CreatedAt = DateTimeOffset.Now.UtcDateTime;
            ExternalPhotoAlbumUrl = externalPhotoAlbumUrl;
            EventId = relatedEventId;
            TeaserImage = photoIdentifiers?.OriginalPhotoIdentifier?.Serialze();
            TeaserImageThumb = photoIdentifiers?.ThumbnailPhotoIdentifier?.Serialze();
            
            RaiseDomainEvent(new LogbookEntryCreatedEvent(Id, title));
        }
        
        internal LogbookEntry()
        {
        }

        [Required] public string Title { get; internal set; } = "";
        [Required] public string Text { get; internal set; } = "";
        [Required] public string TeaserText { get; internal set; } = "";
        public bool IsFavorite { get; internal set; }
        public string TeaserImage { get; internal set; }
        public string TeaserImageThumb { get; internal set; }
        public string ExternalPhotoAlbumUrl { get; internal set; }
        [Required] public DateTime CreatedAt { get; internal set; }
        public DateTime? ModifiedAt { get; internal set; }
        public bool IsPublished { get; internal set; }
        public Guid? EditorAuthorId { [UsedImplicitly] get; internal set; }
        public Diver EditorAuthor { get; [UsedImplicitly] internal set; }
        [Required] public Guid OriginalAuthorId { get; [UsedImplicitly] private set; }
        public Diver OriginalAuthor { get; internal set; }
        public Guid? EventId { get; private set; }
        public Event Event { get; [UsedImplicitly] internal set; }

        public void Publish()
        {
            if (IsPublished) return;

            IsPublished = true;
            RaiseDomainEvent(new LogbookEntryPublishedEvent(Id));
        }

        public void Unpublish()
        {
            if (!IsPublished) return;

            IsPublished = false;
            RaiseDomainEvent(new LogbookEntryUnpublishedEvent(Id));
        }

        public void Edit(
            [NotNull] string title,
            [NotNull] string teaser,
            [NotNull] string text,
            bool isFavorite,
            Guid editorDiverId,
            [CanBeNull] string externalPhotoAlbumUrl,
            Guid? relatedEventId,
            [CanBeNull] PhotoAndThumbnailIdentifiers photoIdentifiers)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
            if (string.IsNullOrWhiteSpace(teaser))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(teaser));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

            Title = title;
            TeaserText = teaser;
            Text = text;
            IsFavorite = isFavorite;
            EditorAuthorId = editorDiverId;
            ModifiedAt = DateTimeOffset.UtcNow.DateTime;
            ExternalPhotoAlbumUrl = externalPhotoAlbumUrl;
            EventId = relatedEventId;
            if (photoIdentifiers != null && (photoIdentifiers.OriginalPhotoIdentifier != null ||
                                             photoIdentifiers.ThumbnailPhotoIdentifier != null))
            {
                TeaserImage = photoIdentifiers.OriginalPhotoIdentifier?.Serialze();
                TeaserImageThumb = photoIdentifiers.ThumbnailPhotoIdentifier?.Serialze();
            }
            
            RaiseDomainEvent(new LogbookEntryEditedEvent(Id, Title));
        }

        public void Delete()
        {
            RaiseDomainEvent(new LogbookEntryDeletedEvent(Id, TeaserImage, TeaserImageThumb));
        }
    }
}