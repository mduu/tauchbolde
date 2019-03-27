using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Common;

namespace Tauchbolde.Web.Core
{
    /// <summary>
    /// Base class for all Tauchbolde.Web controllers.
    /// </summary>
    public abstract class AppControllerBase: Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IDiverRepository diverRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Web.Core.AppControllerBase"/> class.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="diverRepository">Diver repository.</param>
        protected AppControllerBase(
            UserManager<IdentityUser> userManager,
            IDiverRepository diverRepository)
        {
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
        protected void ShowWarningMessage([CanBeNull] string message)
        {
             TempData["warning_message"] = message;
        }

        /// <summary>
        /// Gets the diver for the currently logged in user.
        /// </summary>
        /// <returns>The diver for current user.</returns>
        [NotNull]
        protected async Task<Diver> GetDiverForCurrentUserAsync()
        {
            return User?.Identity?.Name != null
                ? await diverRepository.FindByUserNameAsync(User.Identity.Name)
                : null;
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="diver"/> is an administrator;
        /// otherwise <c>false</c> is returned.
        /// </summary>
        /// <returns>The is admin.</returns>
        /// <param name="diver">Diver.</param>
        protected async Task<bool> GetIsAdmin([CanBeNull] Diver diver)
        {
            return diver != null && await userManager.IsInRoleAsync(diver.User, Rolenames.Administrator);
        }
        
        /// <summary>
        /// Returns <c>true</c> if the <paramref name="diver"/> id s Tauchbold member.
        /// </summary>
        /// <param name="diver"></param>
        /// <returns><c>True</c> if the <paramref name="diver"/> id s Tauchbold member; otherwise <c>False</c> is returned.</returns>
        protected async Task<bool> GetIsTauchbold([CanBeNull] Diver diver)
        {
            return diver != null && await userManager.IsInRoleAsync(diver.User, Rolenames.Tauchbold);
        }
    }
}
