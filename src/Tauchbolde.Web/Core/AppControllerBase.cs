using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Tauchbolde.Web.Core
{
    /// <summary>
    /// Base class for all Tauchbolde.Web controllers.
    /// </summary>
    public abstract class AppControllerBase: Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Web.Core.AppControllerBase"/> class.
        /// </summary>
        protected AppControllerBase()
        {
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
    }
}
