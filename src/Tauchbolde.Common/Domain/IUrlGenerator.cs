using System;
using JetBrains.Annotations;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain
{
    public interface IUrlGenerator
    {
        /// <summary>
        /// Generates the absolute URL to an event.
        /// </summary>
        /// <returns>The event URL.</returns>
        /// <param name="baseUrl">The absolute base Url to prefix to the generated relative URl with.</param>
        /// <param name="eventId">The <see cref="Event.Id"/>.</param>
        string GenerateEventUrl([NotNull]string baseUrl, Guid eventId);

        /// <summary>
        /// Generates the absolute URL for a logbook entry.
        /// </summary>
        /// <param name="baseUrl">The absolute base Url to prefix to the generated relative URl with.</param>
        /// <param name="logbookEntryId">The <see cref="LogbookEntry.Id"/> to generate the Url for.</param>
        /// <returns></returns>
        string GenerateLogbookEntryUrl([NotNull] string baseUrl, Guid logbookEntryId);
    }
}
