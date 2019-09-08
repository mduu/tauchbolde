using System;
using System.ComponentModel.DataAnnotations;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    // TODO Make the prop setters "internal"
    public class Participant : EntityBase
    {
        // TODO Make this constructor "internal"
        public Participant()
        {
        }

        public Participant(
            Guid eventId,
            Guid diverId,
            ParticipantStatus status,
            string buddyTeamName,
            int numberOfPeople,
            string note)
        {
            Id = Guid.NewGuid();
            EventId = eventId;
            ParticipatingDiverId = diverId;
            Status = status;
            BuddyTeamName = buddyTeamName;
            CountPeople = Math.Max(1, numberOfPeople);
            Note = note;
            
            RaiseDomainEvent(new ParticipationChangedEvent(Id, EventId, ParticipatingDiverId, Status));
        }
        
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

        public void Edit(ParticipantStatus status, string buddyTeamName, int numberOfPeople, string note)
        {
            Status = status;
            BuddyTeamName = buddyTeamName;
            CountPeople = Math.Max(1, numberOfPeople);
            Note = note;
            
            RaiseDomainEvent(new ParticipationChangedEvent(Id, EventId, ParticipatingDiverId, Status));
        }
    }
}
