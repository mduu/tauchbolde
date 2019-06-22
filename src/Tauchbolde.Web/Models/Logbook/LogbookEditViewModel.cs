using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Tauchbolde.Web.Models.Logbook
{
    public class LogbookEditViewModel
    {
        public Guid? Id { get; set; }
        
        [DisplayName("Titel")]
        [Required]
        [NotNull]
        public string Title { get; set; } = "";
        
        [DisplayName("Featured")]
        public bool IsFavorite { get; set; }
        
        [DisplayName("Titelbild")]
        [CanBeNull]
        public IFormFile TeaserImage { get; set; }
        
        [DisplayName("Einleitungstext (optional)")]
        [CanBeNull]
        public string Teaser { get; set; }
        
        [DisplayName("Text")]
        [Required]
        [NotNull]
        public string Text { get; set; }
        
        [DisplayName("Link zu externem Fotoalbum (optional)")]
        [CanBeNull]
        public string ExternalPhotoAlbumUrl { get; set; }

        [DisplayName("Erstellt am")]
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}