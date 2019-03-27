using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Tauchbolde.Common.Model
{
    /// <summary>
    /// Entity model for Entries in the Logbook.
    /// </summary>
    [UsedImplicitly]
    public class LogbookEntry
    {
        [Key]
        [DisplayName("Logbuch ID")]
        public Guid Id { get; set; }

        [DisplayName("Titel")]
        [Required]
        [NotNull] public string Title { get; set; } = "";

        [DisplayName("Text/Beschreibung")]
        [Required]
        [NotNull] public string Text { get; set; } = "";

        [DisplayName("Optionaler Teaser/Intro")]
        [Required]
        [NotNull] public string TeaserText { get; set; } = "";

        [DisplayName("Favorisierter Eintrag")]
        public bool IsFavorite { get; set; } = false;

        [CanBeNull] public string TeaserImage { get; set; }

        [CanBeNull] public string TeaserImageThumb { get; set; }

        [DisplayName("Optionale Url externer Fotoalbum")]
        [CanBeNull] public string ExternalPhotoAlbumUrl { get; set; }

        [DisplayName("Erstellt am")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [DisplayName("Geändert am")]
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// ID of the Author that last modified this <see cref="LogbookEntry"/>.
        /// </summary>
        public Guid? EditorAuthorId { get; set; }
        /// <summary>
        /// Author that last modified this <see cref="LogbookEntry"/>.
        /// </summary>
        public Diver EditorAuthor { get; set; }

        /// <summary>
        /// ID of the Original author that initially created this <see cref="LogbookEntry"/>.
        /// </summary>
        [Required]
        [UsedImplicitly]
        public Guid OriginalAuthorId { get; set; }
        /// <summary>
        /// Original author that initially created this <see cref="LogbookEntry"/>.
        /// </summary>
        public Diver OriginalAuthor { get; set; }

        public Guid? EventId { get; set; }
        [CanBeNull] public Event Event { get; set; }
    }
}