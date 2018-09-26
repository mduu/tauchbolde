using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Tauchbolde.Common.Model
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Fullname { get; set; }

        [Display(Name = "Vorname")]
        [Required]
        public string Firstname { get; set; }

        [Display(Name = "Nachname")]
        [Required]
        public string Lastname { get; set; }

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

        [Display(Name = "Benachrichtigungen zuletzt geprüft um")]
        public DateTime? LastNotificationCheckAt { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [NotMapped]
        public string Realname => string.IsNullOrWhiteSpace(Fullname) ? User.UserName : Fullname;

        public virtual ICollection<Notification> Notificationses { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole<int>> Roles { get; } = new List<IdentityUserRole<int>>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<int>> Claims { get; } = new List<IdentityUserClaim<int>>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<int>> Logins { get; } = new List<IdentityUserLogin<int>>();


    }
}
