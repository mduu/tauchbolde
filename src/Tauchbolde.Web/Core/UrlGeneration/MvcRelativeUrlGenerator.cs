using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Tauchbolde.InterfaceAdapters;

namespace Tauchbolde.Web.Core.UrlGeneration
{
    [UsedImplicitly]
    public class MvcRelativeUrlGenerator : MvcUrlGeneratorBase, IRelativeUrlGenerator
    {
        public MvcRelativeUrlGenerator(
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        : base(urlHelperFactory, actionContextAccessor)
        {
        }
        
        public string GenerateEventUrl(Guid? eventId) => 
            eventId.HasValue
                ? GetControllerActionUri("Details", "Event", new {Id = eventId})
                : null;

        public string GenerateLogbookEntryUrl(Guid? logbookEntryId) =>
            logbookEntryId != null
                ? GetControllerActionUri(
                    "Details",
                    "Logbook",
                    new {Id = logbookEntryId})
                : null;
    }
}