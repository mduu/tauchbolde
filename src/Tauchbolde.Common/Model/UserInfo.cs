using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Tauchbolde.Common.Model
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        [Display(Name = "Tauchbold seit")]
        public DateTime? MemberSince { get; set; }

        [Display(Name = "Tauchbolde verlassen am")]
        public DateTime? MemberUntil { get; set; }

        [Display(Name = "Webseite")]
        public string WebsiteUrl { get; set; }

        [Display(Name = "Twitter Handle")]
        public string TwitterHandle { get; set; }

        [Display(Name = "Skype ID")]
        public string SkypeId { get; set; }

        [Display(Name = "Motto")]
        public string Slogan { get; set; }

        [Display(Name = "Ausbildung")]
        public string Education { get; set; }

        [Display(Name = "Erfahrung (Anzahl TG's)")]
        public string Experience { get; set; }

        [Display(Name = "Mobile")]
        public string MobilePhone { get; set; }

        [Display(Name = "Benachrichtigungsintervall (in Stunden)")]
        [Required]
        public int NotificationIntervalInHours { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
