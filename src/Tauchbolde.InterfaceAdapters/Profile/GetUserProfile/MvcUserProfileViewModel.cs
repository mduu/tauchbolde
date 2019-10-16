using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.InterfaceAdapters.Profile.GetUserProfile
{
    public class MvcUserProfileViewModel
    {
        public MvcUserProfileViewModel(
            bool allowEdit,
            Guid userId,
            IEnumerable<string> roles,
            string email,
            string emailUrl,
            string avatarId,
            string realname,
            string firstname,
            string lastname,
            string memberSince,
            string slogan,
            string education,
            string experience,
            string mobilePhone,
            string mobilePhoneUrl,
            string websiteUrl,
            string twitterHandle,
            string twitterUrl,
            string facebookId,
            string facebookUrl,
            string skypeId,
            string skypeUrl)
        {
            AllowEdit = allowEdit;
            UserId = userId;
            Roles = roles;
            Email = email;
            EmailUrl = emailUrl;
            AvatarId = avatarId;
            Realname = realname;
            Firstname = firstname;
            Lastname = lastname;
            MemberSince = memberSince;
            Slogan = slogan;
            Education = education;
            Experience = experience;
            MobilePhone = mobilePhone;
            MobilePhoneUrl = mobilePhoneUrl;
            WebsiteUrl = websiteUrl;
            TwitterHandle = twitterHandle;
            TwitterUrl = twitterUrl;
            FacebookId = facebookId;
            FacebookUrl = facebookUrl;
            SkypeId = skypeId;
            SkypeUrl = skypeUrl;
        }

        public bool AllowEdit { get; }
        public Guid UserId { get; }
        public IEnumerable<string> Roles { get; }
        [Display(Name = "Email")]
        public string Email { get; }
        public string EmailUrl { get; }
        [Display(Name = "Avatar ID")]
        public string AvatarId { get; }
        [Display(Name = "Name")]
        public string Realname { get; }
        [Display(Name = "Vorname")]
        public string Firstname { get; }
        [Display(Name = "Nachname")]
        public string Lastname { get; }
        [Display(Name = "Tauchbold seit")]
        public string MemberSince { get; }
        [Display(Name = "Motto")]
        public string Slogan { get; }
        [Display(Name = "Ausbildung")]
        public string Education { get; }
        [Display(Name = "Erfahrung (Anzahl TG's)")]
        public string Experience { get; }
        [Display(Name = "Mobile")]
        public string MobilePhone { get; }
        public string MobilePhoneUrl { get; }
        [Display(Name = "Webseite")]
        public string WebsiteUrl { get; }
        [Display(Name = "Twitter Handle")]
        public string TwitterHandle { get; }
        public string TwitterUrl { get; }
        [Display(Name = "Facebookname/-id")]
        public string FacebookId { get; }
        public string FacebookUrl { get; }
        [Display(Name = "Skype ID")]
        public string SkypeId { get; }
        public string SkypeUrl { get; }
    }
}