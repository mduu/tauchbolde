using System.Collections.Generic;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class RecentNewsViewModel
    {
        public ICollection<Post> RecentNews { get; set; }
    }
}
