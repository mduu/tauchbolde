using System;
using System.Collections.Generic;
using System.Text;

namespace Tauchbolde.Common.Model
{
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }
        public int CountPeople { get; set; }
        public string Comment { get; set; }
        public ParticipantStatus Status { get; set; }
        public string BuddyTeamName { get; set; }
    }
}
