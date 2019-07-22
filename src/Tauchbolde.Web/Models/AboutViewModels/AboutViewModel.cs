using System.Collections.Generic;
using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Web.Models.AboutViewModels
{
    public class AboutViewModel
    {
        public bool IsTauchbold { get; set; }
        public ICollection<Diver> Members;
    }
}
