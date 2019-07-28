using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities
{
    [Table("LogbookEntries")]
    public class LogbookEntryData : EntityBase
    {
        [Required] public string Title { get; set; } = "";
        [Required] public string Text { get; set; } = "";
        [Required] public string TeaserText { get; set; } = "";
        public bool IsFavorite { get; set; } = false;
        public string TeaserImage { get; set; }
        public string TeaserImageThumb { get; set; }
        public string ExternalPhotoAlbumUrl { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsPublished { get; set; }
        public Guid? EditorAuthorId { get; set; }
        public DiverData EditorAuthor { get; set; }
        [Required] public Guid OriginalAuthorId { get; set; }
        public DiverData OriginalAuthor { get; set; }
        public Guid? EventId { get; set; }
        public EventData Event { get; set; }
    }
}