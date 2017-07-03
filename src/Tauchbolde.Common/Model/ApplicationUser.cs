using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Tauchbolde.Common.Model
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public UserInfo AdditionalUserInfos { get; set; }
        public virtual ICollection<Notification> Notificationses { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        [NotMapped]
        public string Realname => string.IsNullOrWhiteSpace(AdditionalUserInfos?.Fullname) ? UserName : AdditionalUserInfos?.Fullname;
    }
}
