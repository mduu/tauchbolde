using System;
using JetBrains.Annotations;
using Tauchbolde.Common.Model;

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
        [CanBeNull] public string Teaser { get; set; }
        [CanBeNull] public string Text { get; set; }
        [CanBeNull] public string ExternalPhotoAlbumUrl { get; set; }
        [CanBeNull] public string EventTitel { get; set; }
        
        public string OriginalAuthorName { get; set; }
        [NotNull] public Diver OriginalAuthor { get; set; } = new Diver();
        [NotNull] public string CreatedAt { get; set; } = null;

        [CanBeNull] public string EditorAuthorName { get; set; }
        [CanBeNull] public Diver EditorAuthor { get; set; }
        [CanBeNull] public string EditedAt { get; set; }

        [CanBeNull] public string EditUrl { get; set; }
        [CanBeNull] public string EventUrl { get; set; }
    }
}