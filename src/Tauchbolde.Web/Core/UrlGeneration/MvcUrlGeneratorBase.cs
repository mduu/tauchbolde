using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Tauchbolde.Web.Core.UrlGeneration
{
    public abstract class MvcUrlGeneratorBase
    {
        [NotNull] private readonly IUrlHelperFactory urlHelperFactory;
        [NotNull] private readonly IActionContextAccessor actionContextAccessor;

        protected MvcUrlGeneratorBase(
            [NotNull] IUrlHelperFactory urlHelperFactory,
            [NotNull] IActionContextAccessor actionContextAccessor)
        {
            this.urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
            this.actionContextAccessor = actionContextAccessor ?? throw new ArgumentNullException(nameof(actionContextAccessor));
        }

        protected string GetControllerActionUri(string actionName, string controllerName, object routeValues)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            return urlHelper.Action(actionName, controllerName, routeValues);
        }
    }
}