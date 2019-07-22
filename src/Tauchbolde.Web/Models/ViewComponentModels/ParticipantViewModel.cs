using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class ParticipantViewModel
    {
        public Diver Diver { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int CountPeople { get; set; }
        public ParticipantStatus Status { get; set; }
    }
}
