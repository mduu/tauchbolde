using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Tauchbolde.InterfaceAdapters;

namespace Tauchbolde.Web.Core.UrlGeneration
{
    /// <summary>
    /// Implement <see cref="IAbsoluteUrlGenerator"/> using IUrlHelper from ASP.Net MVC.
    /// </summary>
    public class MvcAbsoluteUrlGenerator : MvcUrlGeneratorBase, IAbsoluteUrlGenerator
    {
        public MvcAbsoluteUrlGenerator(
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
            : base(urlHelperFactory, actionContextAccessor)
        {
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
            var relativeActionUrl = base.GetControllerActionUri(actionName, controllerName, routeValues);

            return new Uri(new Uri(baseUrl), relativeActionUrl);
        }
    }
}