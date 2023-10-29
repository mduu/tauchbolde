using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.GetEditUserProfile
{
    public class MvcEditUserProfileViewModel
    {
        public MvcEditUserProfileViewModel(
            Guid userId,
            string username,
            string fullname,
            string firstname,
            string lastname,
            string slogan,
            string education,
            string experience,
            string mobilePhone,
            string websiteUrl,
            string twitterHandle,
            string facebookId,
            string skypeId)
        {
            UserId = userId;
            Username = username;
            Fullname = fullname;
            Firstname = firstname;
            Lastname = lastname;
            Slogan = slogan;
            Education = education;
            Experience = experience;
            MobilePhone = mobilePhone;
            WebsiteUrl = websiteUrl;
            TwitterHandle = twitterHandle;
            FacebookId = facebookId;
            SkypeId = skypeId;
        }

        [UsedImplicitly]
        public MvcEditUserProfileViewModel()
        {
        }
        
        [Required] public Guid UserId { get; set; }
        [Required] public string Username { get; set; }
        [Display(Name = "Name")] [Required] public string Fullname { get; set; }
        [Display(Name = "Vorname")] [Required] public string Firstname { get; set; }
        [Display(Name = "Nachname")] [Required] public string Lastname { get; set; }
        [Display(Name = "Motto")] public string Slogan { get; set; }
        [Display(Name = "Ausbildung")] public string Education { get; set; }
        [Display(Name = "Erfahrung (Anzahl TG's)")] public string Experience { get; set; }
        [Display(Name = "Mobile")] public string MobilePhone { get; set; }
        [Display(Name = "Webseite")] public string WebsiteUrl { get; set; }
        [Display(Name = "Twitter Handle")] public string TwitterHandle { get; set; }
        [Display(Name = "Facebookname/-id")] public string FacebookId { get; set; }
        [Display(Name = "Skype ID")] public string SkypeId { get; set; }
    }
}