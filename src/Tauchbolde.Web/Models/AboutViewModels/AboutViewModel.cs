using System;
using System.Collections.Generic;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.AboutViewModels
{
    public class AboutViewModel
    {
        public bool IsTauchbold { get; set; }
        public ICollection<Diver> Members;
    }
}
