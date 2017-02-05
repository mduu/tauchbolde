using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Tauchbolde.Common.Model
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public UserInfo AdditionalUserInfos { get; set; }
        public virtual ICollection<Notifications> Notificationses { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
