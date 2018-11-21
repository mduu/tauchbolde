using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace Tauchbolde.Web.Core
{
    /// <summary>
    /// Base class for all Tauchbolde.Web controllers.
    /// </summary>
    public abstract class AppControllerBase: Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IDiverRepository diverRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Web.Core.AppControllerBase"/> class.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="diverRepository">Diver repository.</param>
        public AppControllerBase(
            UserManager<IdentityUser> userManager,
            IDiverRepository diverRepository)
        {
            this.hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }

        /// <summary>
        /// Shows a success message on the next page rendering.
        /// </summary>
        /// <param name="message">Message to show.</param>
        protected void ShowSuccessMessage(string message)
        {
            TempData["success_message"] = message;
        }
        
        /// <summary>
        /// Shows a error message on the next page rendering.
        /// </summary>
        /// <param name="message">Error message to show.</param>
        protected void ShowErrorMessage(string message)
        {
            TempData["error_message"] = message;
        }
        
        /// <summary>
        /// Shows a warning message on the next page rendering.
        /// </summary>
        /// <param name="message">The warning message to show.</param>
        protected void ShowWarningMessage(string message)
        {
             TempData["warning_message"] = message;
        }

        /// <summary>
        /// Gets the diver for the currently logged in user.
        /// </summary>
        /// <returns>The diver for current user.</returns>
        protected async Task<Diver> GetDiverForCurrentUser()
        {
            return await diverRepository.FindByUserNameAsync(User.Identity.Name);
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="diver"/> is an administrator;
        /// otherwise <c>false</c> is returned.
        /// </summary>
        /// <returns>The is admin.</returns>
        /// <param name="diver">Diver.</param>
        protected async Task<bool> GetIsAdmin(Diver diver)
        {
            return await userManager.IsInRoleAsync(diver.User, Rolenames.Administrator);
        }
    }
}
