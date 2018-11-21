using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Model;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public enum AvatarSize
    {
        Small,
        Medium,
    }

    public class AvatarViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(Diver diver, AvatarSize avatarSize)
        {
            if (diver == null) { throw new ArgumentNullException(nameof(diver)); }

            return View(new AvatarViewModel
            {
                Diver = diver,
                AvatarSize = avatarSize,
            });
        }
    }
}
