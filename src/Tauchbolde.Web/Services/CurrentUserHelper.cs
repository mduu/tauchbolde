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
        /// <param name="applicationUserRepository">The <see cref="IDiverRepository"/> used to access the database.</param>
        /// <returns>Return the <see cref="Diver"/> instance or <c>Null</c> if none was found.</returns>
        public static async Task<Diver> GetCurrentUserAsync(this Controller controller, IDiverRepository applicationUserRepository)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (applicationUserRepository == null) throw new ArgumentNullException(nameof(applicationUserRepository));

            return await applicationUserRepository.FindByUserNameAsync(controller.User.Identity.Name);
        }
    }
}
