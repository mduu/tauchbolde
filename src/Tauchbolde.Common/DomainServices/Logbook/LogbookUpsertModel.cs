using System;
using System.IO;
using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.Logbook
{
    public class LogbookUpsertModel
    {
        public Guid? Id { get; set; }
        [NotNull] public string Title { get; set; } = "";
        [NotNull] public string Text { get; set; } = "";
        public bool IsFavorite { get; set; }
        [CanBeNull]
        public Stream TeaserImage { get; set; }
        [CanBeNull] public string Teaser { get; set; }
        [CanBeNull] public string ExternalPhotoAlbumUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CurrentDiverId { get; set; }
  }
}