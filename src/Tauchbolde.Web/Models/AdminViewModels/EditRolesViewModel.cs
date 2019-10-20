using System.Collections.Generic;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Web.Models.AdminViewModels
{
    public class EditRolesViewModel
    {
        public Diver Profile { get; set; }
        public ICollection<string> AssignedRoles { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
