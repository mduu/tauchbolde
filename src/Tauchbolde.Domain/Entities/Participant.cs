using System;
using System.ComponentModel.DataAnnotations;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    public class Participant : EntityBase
    {
        [Display(Name = "Anlass ID")]
        [Required]
        public Guid EventId { get; set; }
        
        [Display(Name = "Anlass")]
        [Required]
        public Event Event { get; set; }

        public Guid ParticipatingDiverId { get; set; }

        [Display(Name = "Teilnehmer")]
        [Required]
        public Diver ParticipatingDiver { get; set; }

        [Display(Name = "Anzahl Personen")]
        [Required]
        public int CountPeople { get; set; }

        [Display(Name ="Notiz")]
        public string Note { get; set; }

        [Display(Name ="Status")]
        [Required]
        public ParticipantStatus Status { get; set; }

        [Display(Name="Buddy Team")]
        public string BuddyTeamName { get; set; }
    }
}
