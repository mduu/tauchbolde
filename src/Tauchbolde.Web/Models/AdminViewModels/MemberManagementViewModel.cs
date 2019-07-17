using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Entities;

namespace Tauchbolde.Web.Models.AdminViewModels
{
    public class MemberManagementViewModel
    {
        public ICollection<MemberViewModel> Members { get; set; }
        public string AddUserId { get; set; }
        public ICollection<IdentityUser> AddableUsers { get; set; }
    }
    
    public class MemberViewModel
    {
        public Diver Profile { get; set; }
        public ICollection<string> Roles { get; set; }

    }
}
