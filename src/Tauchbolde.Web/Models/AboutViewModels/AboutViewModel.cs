using System.Collections.Generic;
using Tauchbolde.Entities;

namespace Tauchbolde.Web.Models.AboutViewModels
{
    public class AboutViewModel
    {
        public bool IsTauchbold { get; set; }
        public ICollection<Diver> Members;
    }
}
