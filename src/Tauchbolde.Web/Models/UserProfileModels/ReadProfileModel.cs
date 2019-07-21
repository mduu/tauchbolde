using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Web.Models.UserProfileModels
{
    public class ReadProfileModel
    {
        public bool AllowEdit { get; set; }
        public Diver Profile { get; set; }
        
        [Display(Name = "Rollen/Rechte")]
        public IEnumerable<string> Roles { get; set; }
    }
}
