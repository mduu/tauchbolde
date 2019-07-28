using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities
{
    [Table("Comments")]
    public class CommentData : EntityBase
    {
        public Guid EventId { get; set; }
        [Required] public EventData Event { get; set; }
        [Required] public Guid AuthorId { get; set; }
        [Required] public DiverData Author { get; set; }
        [Required] public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Required] public string Text { get; set; }
    }
}
