using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Tauchbolde.InterfaceAdapters;

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
        /// <param name="actionContextAccessor">Action context.</param>
        public MvcUrlGenerator(
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            this.urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
            this.actionContextAccessor =
                actionContextAccessor ?? throw new ArgumentNullException(nameof(actionContextAccessor));
        }

        /// <inheritdoc />
        public string GenerateEventUrl(string baseUrl, Guid eventId)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));

            var eventDetailsUri = GetControllerActionUri(
                baseUrl,
                "Details",
                "Event",
                new {Id = eventId});

            return eventDetailsUri.AbsoluteUri;
        }

        /// <inheritdoc />
        public string GenerateLogbookEntryUrl(string baseUrl, Guid logbookEntryId)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));

            var logbookEntryDetailsUri = GetControllerActionUri(
                baseUrl,
                "Details",
                "Logbook",
                new {Id = logbookEntryId});

            return logbookEntryDetailsUri.AbsoluteUri;
        }

        private Uri GetControllerActionUri(string baseUrl, string actionName, string controllerName, object routeValues)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            var relativeActionUrl = urlHelper.Action(actionName, controllerName, routeValues);

            return new Uri(new Uri(baseUrl), relativeActionUrl);
        }
    }
}