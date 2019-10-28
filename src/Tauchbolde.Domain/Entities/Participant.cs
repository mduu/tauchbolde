using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    public class Participant : EntityBase
    {
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
        
        internal Participant()
        {
        }
        
        [Required] public Guid EventId { get; [UsedImplicitly] internal set; }
        [Required] public Event Event { get; [UsedImplicitly] internal set; }
        public Guid ParticipatingDiverId { get; [UsedImplicitly] internal set; }
        [Required] public Diver ParticipatingDiver { get; [UsedImplicitly] internal set; }
        [Required] public int CountPeople { get; [UsedImplicitly] internal set; }
        public string Note { get; [UsedImplicitly] internal set; }
        [Required] public ParticipantStatus Status { get; [UsedImplicitly] internal set; }
        public string BuddyTeamName { get; [UsedImplicitly] internal set; }

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
