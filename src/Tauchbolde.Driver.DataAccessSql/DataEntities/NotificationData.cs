using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities
{
    [Table("Notifications")]
    public class NotificationData : EntityBase
    {
        [Required] public Guid RecipientId { get; set; }
        [Required] public DiverData Recipient { get; set; }
        [Required] public DateTime OccuredAt { get; set; }
        [Required] public bool AlreadySent { get; set; }
        [Required] public int CountOfTries { get; set; }
        [Required] public string Message { get; set; }
        [Required] public NotificationType Type { get; set; }
        public Guid? EventId { get; set; }
        public virtual EventData Event { get; set; }
        public Guid? LogbookEntryId { get; set; }
        public virtual LogbookEntryData LogbookEntry { get; set; }
    }
}
