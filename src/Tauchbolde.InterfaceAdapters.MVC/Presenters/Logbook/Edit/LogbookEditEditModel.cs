using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Logbook.Edit
{
    public class LogbookEditEditModel
    {
        public Guid? Id { get; [UsedImplicitly] set; }
        [Required] public string Title { get; [UsedImplicitly] set; } = "";
        public bool IsFavorite { get; [UsedImplicitly] set; }
        public IFormFile TeaserImage { get; [UsedImplicitly] set; }
        public string Teaser { get; [UsedImplicitly] set; }
        [Required] public string Text { get; [UsedImplicitly] set; }
        public string ExternalPhotoAlbumUrl { get; [UsedImplicitly] set; }
        [Required] public DateTime CreatedAt { get; [UsedImplicitly] set; }
    }
}