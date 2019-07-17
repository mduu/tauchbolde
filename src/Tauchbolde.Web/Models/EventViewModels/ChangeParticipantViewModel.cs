using System;
using System.ComponentModel.DataAnnotations;
using Tauchbolde.Entities;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class ChangeParticipantViewModel
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public int CountPeople { get; set; }

        public string Note { get; set; }

        [Required]
        public ParticipantStatus Status { get; set; }

        public string BuddyTeamName { get; set; }
    }
}
