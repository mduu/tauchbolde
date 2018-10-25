using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using System.Collections;

namespace Tauchbolde.Web.Models.AdminViewModels
{
    public class EditRolesViewModel
    {
        public Diver Profile { get; set; }
        public ICollection<string> AssignedRoles { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
