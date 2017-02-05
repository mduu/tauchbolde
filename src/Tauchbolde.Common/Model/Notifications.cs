using System;
using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Common.Model
{
    public class Notifications
    {
        public Guid Id { get; set; }

        [Display(Name = "Empfänger")]
        [Required]
        public ApplicationUser Recipient { get; set; }

        [Display(Name = "Ereigniszeit")]
        [Required]
        public DateTime OccuredAt { get; set; }

        [Display(Name = "Bereits gesendet")]
        [Required]
        public bool AlreadySent { get; set; }

        [Display(Name = "Anzahl Sendeversuche")]
        [Required]
        public int CountOfTries { get; set; }

        [Display(Name = "Nachricht")]
        [Required]
        public string Message { get; set; }

        [Display(Name = "Nachrichtentyp")]
        [Required]
        public NotificationType Type { get; set; }

        [Display(Name = "Anlass ID")]
        public Guid EventId { get; set; }
        [Display(Name = "Anlass")]
        public virtual Event Event { get; set; }
    }
}
