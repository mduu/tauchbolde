using System;

namespace Tauchbolde.InterfaceAdapters.Profile.GetEditAvatar
{
    public class MvcGetEditAvatarViewModel
    {
        public MvcGetEditAvatarViewModel(Guid userId, string realname)
        {
            UserId = userId;
            Realname = realname;
        }

        public Guid UserId { get; }
        public string Realname { get; }
    }
}