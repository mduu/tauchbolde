using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Application.Services.Core;

namespace Tauchbolde.Web.Filters
{
    public class CurrentUserInformationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var currentUserInformation = context.HttpContext.RequestServices.GetService<ICurrentUserInformation>();
            currentUserInformation.UserName = context.HttpContext.User?.Identity?.IsAuthenticated == true
                ? context.HttpContext.User?.Identity?.Name
                : null;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}