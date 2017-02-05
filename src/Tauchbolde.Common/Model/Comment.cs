using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Common.Model
{
    public class Comment
    {
        public Guid Id { get; set; }

        [Display(Name = "Anlass ID")]
        [Required]
        public Guid EventId { get; set; }
        [Display(Name = "Anlass")]
        [Required]
        public Event Event { get; set; }

        [Display(Name = "Autor")]
        [Required]
        public ApplicationUser Author { get; set; }

        [Display(Name = "ID übergeordneter Kommentar")]
        public Guid ParentCommentId { get; set; }
        [Display(Name = "Übergeordneter Kommentar")]
        public virtual Comment ParentComment { get; set; }

        [Display(Name = "Untergeordnete Kommentare")]
        [Required]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
