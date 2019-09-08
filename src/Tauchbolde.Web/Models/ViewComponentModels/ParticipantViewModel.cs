using System;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class ParticipantViewModel
    {
        public string DiverEmail { get; set; }
        public Guid DiverId { get; set; }
        public string DiverAvatarId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int CountPeople { get; set; }
        public ParticipantStatus Status { get; set; }
    }
}
