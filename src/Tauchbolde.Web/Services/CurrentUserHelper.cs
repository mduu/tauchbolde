using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Web.Services
{
    /// <summary>
    /// Little helpers for current user related functions.
    /// </summary>
    public static class CurrentUserHelper
    {
        /// <summary>
        /// Get the user enttity instance for the currenctly logged in user.
        /// </summary>
        /// <param name="controller">The MVC controller used to access the identity.</param>
        /// <param name="applicationUserRepository">The <see cref="IApplicationUserRepository"/> used to access the database.</param>
        /// <returns>Return the <see cref="UserInfo"/> instance or <c>Null</c> if none was found.</returns>
        public static async Task<UserInfo> GetCurrentUserAsync(this Controller controller, IApplicationUserRepository applicationUserRepository)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (applicationUserRepository == null) throw new ArgumentNullException(nameof(applicationUserRepository));

            return await applicationUserRepository.FindByUserNameAsync(controller.User.Identity.Name);
        }
    }
}
