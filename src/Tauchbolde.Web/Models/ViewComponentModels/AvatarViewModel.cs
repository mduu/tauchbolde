using Tauchbolde.Web.ViewComponents;
namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class AvatarViewModel
    {
        public AvatarViewModel(string diverEmail, string avatarId, AvatarSize avatarSize = AvatarSize.Small)
        {
            DiverEmail = diverEmail;
            AvatarId = avatarId;
            AvatarSize = avatarSize;
        }

        public string DiverEmail { get; }
        public string AvatarId { get; }
        public AvatarSize AvatarSize { get; }
    }
}
