using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Logbook.Edit
{
    public class LogbookEditViewModel
    {
        public LogbookEditViewModel(Guid? id,
            bool isFavorite,
            [CanBeNull] IFormFile teaserImage,
            string title,
            [CanBeNull] string teaser,
            [NotNull] string text,
            [CanBeNull] string externalPhotoAlbumUrl,
            DateTime createdAt)
        {
            Id = id;
            IsFavorite = isFavorite;
            TeaserImage = teaserImage;
            Title = title;
            Teaser = teaser;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            ExternalPhotoAlbumUrl = externalPhotoAlbumUrl;
            CreatedAt = createdAt;
        }

        public Guid? Id { get; }
        
        [DisplayName("Titel")]
        [Required]
        [NotNull]
        public string Title { get; } = "";
        
        [DisplayName("Featured")]
        public bool IsFavorite { get; }
        
        [DisplayName("Titelbild")]
        [CanBeNull]
        public IFormFile TeaserImage { get; }
        
        [DisplayName("Einleitungstext (optional)")]
        [CanBeNull]
        public string Teaser { get; }
        
        [DisplayName("Text")]
        [Required]
        [NotNull]
        public string Text { get; }
        
        [DisplayName("Link zu externem Fotoalbum (optional)")]
        [CanBeNull]
        public string ExternalPhotoAlbumUrl { get; }

        [DisplayName("Erstellt am")]
        [Required]
        public DateTime CreatedAt { get; }

    }
}