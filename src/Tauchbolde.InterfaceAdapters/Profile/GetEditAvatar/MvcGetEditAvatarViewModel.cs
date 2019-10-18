using System;

namespace Tauchbolde.InterfaceAdapters.Profile.GetEditAvatar
{
    public class MvcGetEditAvatarViewModel
    {
        public MvcGetEditAvatarViewModel(
            Guid userId,
            string realname,
            int avatarSizeSm,
            int avatarSizeMd)
        {
            UserId = userId;
            Realname = realname;
            AvatarSizeSm = avatarSizeSm;
            AvatarSizeMd = avatarSizeMd;
        }

        public Guid UserId { get; }
        public string Realname { get; }
        public int AvatarSizeSm { get; }
        public int AvatarSizeMd { get; }
    }
}