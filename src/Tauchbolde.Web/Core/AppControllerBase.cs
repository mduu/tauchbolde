using System;
using Microsoft.AspNetCore.Mvc;
namespace Tauchbolde.Web.Core
{
    public abstract class AppControllerBase: Controller
    {
        protected void ShowSuccessMessage(string message)
        {
            TempData["success_message"] = message;
        }
        
        protected void ShowErrorMessage(string message)
        {
            TempData["error_message"] = message;
        }
    }
}
