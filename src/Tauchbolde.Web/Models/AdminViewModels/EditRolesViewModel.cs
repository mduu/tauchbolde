using System.Collections.Generic;
using Tauchbolde.Entities;

namespace Tauchbolde.Web.Models.AdminViewModels
{
    public class EditRolesViewModel
    {
        public Diver Profile { get; set; }
        public ICollection<string> AssignedRoles { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
