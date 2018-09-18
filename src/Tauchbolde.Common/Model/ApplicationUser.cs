using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Tauchbolde.Common.Model
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {
        public UserInfo AdditionalUserInfos { get; set; }
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

        [NotMapped]
        public string Realname => string.IsNullOrWhiteSpace(AdditionalUserInfos?.Fullname) ? UserName : AdditionalUserInfos?.Fullname;
    }
}
