using System;
using System.ComponentModel.DataAnnotations;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    public class Participant : EntityBase
    {
        internal Participant()
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
            CountPeople = Math.Min(1, numberOfPeople);
            Note = note;
            
            RaiseDomainEvent(new ParticipationChangedEvent(Id, EventId, ParticipatingDiverId, Status));
        }
        
        [Display(Name = "Anlass ID")]
        [Required]
        public Guid EventId { get; internal set; }
        
        [Display(Name = "Anlass")]
        [Required]
        public Event Event { get; internal set; }

        public Guid ParticipatingDiverId { get; internal set; }

        [Display(Name = "Teilnehmer")]
        [Required]
        public Diver ParticipatingDiver { get; internal set; }

        [Display(Name = "Anzahl Personen")]
        [Required]
        public int CountPeople { get; internal set; }

        [Display(Name ="Notiz")]
        public string Note { get; internal set; }

        [Display(Name ="Status")]
        [Required]
        public ParticipantStatus Status { get; internal set; }

        [Display(Name="Buddy Team")]
        public string BuddyTeamName { get; internal set; }

        public void Edit(ParticipantStatus status, string buddyTeamName, int numberOfPeople, string note)
        {
            Status = status;
            BuddyTeamName = buddyTeamName;
            CountPeople = Math.Min(1, numberOfPeople);
            Note = note;
            
            RaiseDomainEvent(new ParticipationChangedEvent(Id, EventId, ParticipatingDiverId, Status));
        }
    }
}
