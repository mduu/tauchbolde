using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Common.Model
{
    public class Post
    {
        public Guid Id { get; set; }

        [Display(Name = "Kategorie")]
        [Required]
        public PostCategory Category { get; set; }

        [Display(Name = "Erstellt am")]
        [Required]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Publiziert am")]
        [Required]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Geändert am")]
        public DateTime ModificationDate { get; set; }

        [Display(Name = "Autor")]
        [Required]
        public ApplicationUser Author { get; set; }

        [Display(Name = "Titel")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Titelbild")]
        public Guid IntroImageId { get; set; }

        [Display(Name = "Bilder")]
        public ICollection<PostImage> PostImages { get; set; }
    }
}
