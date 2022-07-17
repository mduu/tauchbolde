using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Domain.Events.Diver;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Services;

namespace Tauchbolde.Domain.Entities
{
    public class Diver : EntityBase
    {
        [Required] public string Fullname { get; internal set; }
        [Required] public string Firstname { get; internal set; }
        [Required] public string Lastname { get; internal set; }
        public DateTime? MemberSince { get; internal set; }
        public DateTime? MemberUntil { get; internal set; }
        public string AvatarId { get; internal set; }
        public string WebsiteUrl { get; internal set; }
        public string TwitterHandle { get; internal set; }
        public string FacebookId { get; internal set; }
        public string SkypeId { get; internal set; }
        public string Slogan { get; internal set; }
        public string Education { get; internal set; }
        public string Experience { get; internal set; }
        public string MobilePhone { get; internal set; }
        [Required] public int NotificationIntervalInHours { get; internal set; }
        [Required, DefaultValue(false)] public bool SendOwnNoticiations { get; internal set; }
        public DateTime? LastNotificationCheckAt { get; internal set; }
        public string UserId { get; internal set; }
        public IdentityUser User { get; internal set; }
        
        [NotMapped] public string Realname => string.IsNullOrWhiteSpace(Fullname) ? User.UserName : Fullname;

        public virtual ICollection<Notification> Notificationses { get; [UsedImplicitly] private set; }
        public virtual ICollection<Event> Events { get; [UsedImplicitly] private set; }
        public virtual ICollection<Comment> Comments { get; [UsedImplicitly] private set; }
        public virtual ICollection<LogbookEntry> OriginalAuthorOfLogbookEntries { get; [UsedImplicitly] private set; }
        public virtual ICollection<LogbookEntry> EditorAuthorOfLogbookEntries { get; [UsedImplicitly] private set; }

        public Diver(
            [NotNull] IdentityUser identityUser,
            [NotNull] string firstName,
            [NotNull] string lastName)
        {
            if (identityUser == null) throw new ArgumentNullException(nameof(identityUser));
            if (firstName == null) throw new ArgumentNullException(nameof(firstName));
            if (lastName == null) throw new ArgumentNullException(nameof(lastName));

            Id = Guid.NewGuid();
            UserId = identityUser.Id;
            Firstname = firstName;
            Lastname = lastName;
            Fullname = $"{Firstname} {Lastname}";
            MobilePhone = identityUser.PhoneNumber;
        }

        internal Diver()
        { }
        
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

        public void MarkNotificationChecked()
        {
            LastNotificationCheckAt = SystemClock.Now;
        }
    }
}