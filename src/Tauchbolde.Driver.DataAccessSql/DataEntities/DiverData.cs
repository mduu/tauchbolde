using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities
{
    [Table("Divers")]
    public class DiverData : EntityBase
    {
        [Required] public string Fullname { get; set; }
        [Required] public string Firstname { get; set; }
        [Required] public string Lastname { get; set; }
        public DateTime? MemberSince { get; set; }
        public DateTime? MemberUntil { get; set; }
        public string AvatarId { get; set; }
        public string WebsiteUrl { get; set; }
        public string TwitterHandle { get; set; }
        public string FacebookId { get; set; }
        public string SkypeId { get; set; }
        public string Slogan { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string MobilePhone { get; set; }
        [Required] public int NotificationIntervalInHours { get; set; }
        [Required, DefaultValue(false)] public bool SendOwnNoticiations { get; set; }
        public DateTime? LastNotificationCheckAt { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public virtual ICollection<NotificationData> Notificationses { get; set; }
        public virtual ICollection<EventData> Events { get; set; }
        public virtual ICollection<CommentData> Comments { get; set; }
        public virtual ICollection<LogbookEntryData> OriginalAuthorOfLogbookEntries { get; set; }
        public virtual ICollection<LogbookEntryData> EditorAuthorOfLogbookEntries { get; set; }
    }
}
