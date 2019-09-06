using System;
using System.ComponentModel.DataAnnotations;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class ChangeParticipantViewModel
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public int CurrentUserCountPeople { get; set; }

        public string CurrentUserNote { get; set; }

        [Required]
        public ParticipantStatus CurrentUserStatus { get; set; }

        public string CurrentUserBuddyTeamName { get; set; }
    }
}
