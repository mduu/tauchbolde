using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities
{
    [Table("Participants")]
    public class ParticipantData : EntityBase
    {
        [Required] public Guid EventId { get; set; }
        [Required] public EventData Event { get; set; }
        [Required] public Guid ParticipatingDiverId { get; set; }
        [Required] public DiverData ParticipatingDiver { get; set; }
        [Required] public int CountPeople { get; set; }
        public string Note { get; set; }
        [Required] public ParticipantStatus Status { get; set; }
        public string BuddyTeamName { get; set; }
    }
}