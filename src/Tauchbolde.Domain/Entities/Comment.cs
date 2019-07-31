using System;
using System.ComponentModel.DataAnnotations;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    public class Comment : EntityBase
    {
        [Display(Name = "Anlass ID")]
        public Guid EventId { get; set; }
        
        [Display(Name = "Autor ID")]
        [Required]
        public Guid AuthorId { get; set; }

        [Display(Name = "Autor")]
        [Required]
        public Diver Author { get; set; }

        [Display(Name = "Geschrieben um")]
        [Required]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Geändert um")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Kommentar")]
        [Required]
        public string Text { get; set; }
    }
}
