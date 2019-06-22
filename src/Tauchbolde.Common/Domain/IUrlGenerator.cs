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
    }
}
