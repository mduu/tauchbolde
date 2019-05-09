using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Tauchbolde.Common.Domain;

namespace Tauchbolde.Web.Core
{
    /// <summary>
    /// Implement <see cref="IUrlGenerator"/> using IUrlHelper from ASP.Net MVC.
    /// </summary>
    public class MvcUrlGenerator : IUrlGenerator
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Web.Core.MvcUrlGenerator"/> class.
        /// </summary>
        /// <param name="urlHelperFactory">URL helper factory.</param>
        public MvcUrlGenerator(
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            this.urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
            this.actionContextAccessor = actionContextAccessor ?? throw new ArgumentNullException(nameof(actionContextAccessor));
        }

        /// <inheritdoc />
        public string GenerateEventUrl(Guid eventId)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            var scheme = urlHelper.ActionContext.HttpContext.Request.Scheme;
            
            return urlHelper.Action("Details", "Event", new { Id = eventId }, scheme);
        }
    }
}
