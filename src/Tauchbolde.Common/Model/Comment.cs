using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Common.Model
{
    public class Comment
    {
        public Guid Id { get; set; }

        [Display(Name = "Anlass ID")]
        public Guid EventId { get; set; }
        [Display(Name = "Anlass")]
        [Required]
        public Event Event { get; set; }

        [Display(Name = "Autor")]
        [Required]
        public ApplicationUser Author { get; set; }

        [Display(Name = "Kommentar")]
        [Required]
        public string Text { get; set; }
    }
}
