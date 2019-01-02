using System;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices
{
    public interface IUrlGenerator
    {
        /// <summary>
        /// Generates the absolute URL to an event.
        /// </summary>
        /// <returns>The event URL.</returns>
        /// <param name="eventId">The <see cref="Event.Id"/>.</param>
        string GenerateEventUrl(Guid eventId);
    }
}
