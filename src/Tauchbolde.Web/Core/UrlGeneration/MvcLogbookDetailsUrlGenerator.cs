using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Logbook.Details;

namespace Tauchbolde.Web.Core.UrlGeneration
{
    public class MvcLogbookDetailsUrlGenerator : MvcUrlGeneratorBase, ILogbookDetailsUrlGenerator
    {
        public MvcLogbookDetailsUrlGenerator(
            [NotNull] IUrlHelperFactory urlHelperFactory,
            [NotNull] IActionContextAccessor actionContextAccessor)
            : base(urlHelperFactory, actionContextAccessor)
        {
        }

        public string GenerateEditUrl(Guid logbookEntryId) =>
            GetControllerActionUri(
                "Edit",
                "Logbook",
                new { id = logbookEntryId });

        public string GeneratePublishUrl(Guid logbookEntryId) =>
            GetControllerActionUri(
                "Publish",
                "Logbook",
                new { id = logbookEntryId });

        public string GenerateUnPublishUrl(Guid logbookEntryId) =>
            GetControllerActionUri(
                "Unpublish",
                "Logbook",
                new { id = logbookEntryId });

        public string GenerateDeleteUrl(Guid logbookEntryId) =>
            GetControllerActionUri(
                "Delete",
                "Logbook",
                new { id = logbookEntryId });

        public string GenerateTeaserImageUrl(string teaserImageIdentifier) =>
            GetControllerActionUri(
                "Photo",
                "Logbook", 
                new { photoId = teaserImageIdentifier });
    }
}