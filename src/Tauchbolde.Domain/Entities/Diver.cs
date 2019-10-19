using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Domain.Events.Diver;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    // TODO Remove Display-Attributes as soon as props are not used in any view-model any more
    public class Diver : EntityBase
    {
        [Display(Name = "Name")] [Required] public string Fullname { get; set; }

        [Display(Name = "Vorname")] [Required] public string Firstname { get; set; }

        [Display(Name = "Nachname")]
        [Required]
        public string Lastname { get; set; }

        [Display(Name = "Tauchbold seit")] public DateTime? MemberSince { get; set; }

        [Display(Name = "Tauchbolde verlassen am")]
        public DateTime? MemberUntil { get; set; }

        [Display(Name = "Avatar ID")] public string AvatarId { get; set; }

        [Display(Name = "Webseite")] public string WebsiteUrl { get; set; }

        [Display(Name = "Twitter Handle")] public string TwitterHandle { get; set; }

        [Display(Name = "Facebookname/-id")] public string FacebookId { get; set; }

        [Display(Name = "Skype ID")] public string SkypeId { get; set; }

        [Display(Name = "Motto")] public string Slogan { get; set; }

        [Display(Name = "Ausbildung")] public string Education { get; set; }

        [Display(Name = "Erfahrung (Anzahl TG's)")]
        public string Experience { get; set; }

        [Display(Name = "Mobile")] public string MobilePhone { get; set; }

        [Display(Name = "Benachrichtigungsintervall (in Stunden)")]
        [Required]
        public int NotificationIntervalInHours { get; set; }

        [Display(Name = "Eigene Aktionen in meine Benachrichtungen")]
        [Required, DefaultValue(false)]
        public bool SendOwnNoticiations { get; set; }

        [Display(Name = "Benachrichtigungen zuletzt geprüft um")]
        public DateTime? LastNotificationCheckAt { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [NotMapped] public string Realname => string.IsNullOrWhiteSpace(Fullname) ? User.UserName : Fullname;

        public virtual ICollection<Notification> Notificationses { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<LogbookEntry> OriginalAuthorOfLogbookEntries { get; set; }
        public virtual ICollection<LogbookEntry> EditorAuthorOfLogbookEntries { get; set; }

        [Obsolete]
        public string GetTwitterUrl()
        {
            if (string.IsNullOrWhiteSpace(TwitterHandle))
            {
                return "";
            }

            var twitterUrl = TwitterHandle;

            if (TwitterHandle.StartsWith("@", StringComparison.CurrentCulture))
            {
                twitterUrl = TwitterHandle.Substring(1);
            }

            return $"https://twitter.com/{twitterUrl}";
        }

        [Obsolete]
        public string GetFacebookeUrl() =>
            !string.IsNullOrWhiteSpace(FacebookId)
                ? new Uri($"https://facebook.com/{FacebookId}").AbsoluteUri
                : "";

        public void Edit(
            Guid currentUserId,
            string fullname,
            string firstname,
            string lastname,
            string education,
            string experience,
            string slogan,
            string mobilePhone,
            string websiteUrl,
            string facebookId,
            string twitterHandle,
            string skypeId)
        {
            Fullname = fullname;
            Firstname = firstname;
            Lastname = lastname;
            Education = education;
            Experience = experience;
            Slogan = slogan;
            MobilePhone = mobilePhone;
            WebsiteUrl = websiteUrl;
            FacebookId = facebookId;
            TwitterHandle = twitterHandle;
            SkypeId = skypeId;

            RaiseDomainEvent(new UserProfileEditedEvent(Id, currentUserId));
        }

        public void ChangeAvatar(string newAvatarId)
        {
            if (AvatarId == newAvatarId) return;
            
            AvatarId = newAvatarId;
            RaiseDomainEvent(new AvatarChangedEvent(Id));
        }
    }
}