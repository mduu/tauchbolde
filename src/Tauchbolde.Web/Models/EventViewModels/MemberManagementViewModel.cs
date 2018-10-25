using System.Collections.Generic;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class MemberManagementViewModel
    {
        public ICollection<MemberViewModel> Members { get; set; }
    }
    
    public class MemberViewModel
    {
        public Diver Profile { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
