using Microsoft.AspNetCore.Mvc;
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
        public IViewComponentResult Invoke(string diverEmail, string avatarId, AvatarSize avatarSize) => 
            View(new AvatarViewModel(diverEmail, avatarId, avatarSize));
    }
}
