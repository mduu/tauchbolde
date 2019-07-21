using System;
using JetBrains.Annotations;
using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Web.Models.Logbook
{
    /// <summary>
    /// View model used for rendering Logbook views.
    /// </summary>
    public class LogbookDetailViewModel
    {        
        public bool AllowEdit { get; set; }
        public Guid Id { get; set; }
        [NotNull] public string Title { get; set; } = "";
        public bool IsFavorite { get; set; }
        public bool IsPublished { get; set; }
        [CanBeNull] public string Teaser { get; set; }
        [CanBeNull] public string Text { get; set; }
        [CanBeNull] public string ExternalPhotoAlbumUrl { get; set; }
        [CanBeNull] public string EventTitle { get; set; }
        
        public string OriginalAuthorName { get; set; }
        [NotNull] public Diver OriginalAuthor { get; set; } = new Diver();
        [NotNull] public string CreatedAt { get; set; } = null;

        [CanBeNull] public string EditorAuthorName { get; set; }
        [CanBeNull] public Diver EditorAuthor { get; set; }
        [CanBeNull] public string EditedAt { get; set; }

        [CanBeNull] public string EditUrl { get; set; }
        [CanBeNull] public string UnpublishUrl { get; set; }
        [CanBeNull] public string PublishUrl { get; set; }
        [CanBeNull] public string DeleteUrl { get; set; }
        [CanBeNull] public string EventUrl { get; set; }
        [CanBeNull] public string TeaserImageUrl { get; set; }
        [CanBeNull] public string TeaserThumbImageUrl { get; set; }
    }
}