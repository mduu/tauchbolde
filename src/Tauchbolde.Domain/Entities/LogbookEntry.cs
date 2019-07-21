using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Tauchbolde.Domain.Events;
using Tauchbolde.Domain.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    [UsedImplicitly]
    public class LogbookEntry : EntityBase
    {
        [DisplayName("Titel")]
        [Required]
        [NotNull]
        public string Title { get; set; } = "";

        [DisplayName("Text/Beschreibung")]
        [Required]
        [NotNull]
        public string Text { get; set; } = "";

        [DisplayName("Optionaler Teaser/Intro")]
        [Required]
        [NotNull]
        public string TeaserText { get; set; } = "";

        [DisplayName("Favorisierter Eintrag")] public bool IsFavorite { get; set; } = false;

        [CanBeNull] public string TeaserImage { get; set; }

        [CanBeNull] public string TeaserImageThumb { get; set; }

        [DisplayName("Optionale Url externer Fotoalbum")]
        [CanBeNull]
        public string ExternalPhotoAlbumUrl { get; set; }

        [DisplayName("Erstellt am")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Geändert am")] public DateTime? ModifiedAt { get; set; }

        [DisplayName("Publiziert")] public bool IsPublished { get; set; }

        public Guid? EditorAuthorId { get; set; }

        public Diver EditorAuthor { get; set; }

        [Required] [UsedImplicitly] public Guid OriginalAuthorId { get; set; }

        public Diver OriginalAuthor { get; set; }

        public Guid? EventId { get; set; }

        [CanBeNull] public Event Event { get; set; }

        public void Publish()
        {
            if (!IsPublished)
            {
                IsPublished = true;
                RaiseDomainEvent(new LogbookEntryPublishedEvent(Id));
            }
        }

        public void Unpublish()
        {
            if (IsPublished)
            {
                IsPublished = false;
                RaiseDomainEvent(new LogbookEntryUnpublishedEvent(Id));
            }
        }
    }
}