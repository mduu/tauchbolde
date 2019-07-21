using System.Collections.Generic;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices
{
    /// <summary>
    /// Service functions for Mass-Mailings.
    /// </summary>
    public interface IMassMailService
    {
        /// <summary>
        /// Create a list of recipients as a formatted string based on a
        /// collection of <see cref="Diver"/>.
        /// </summary>
        /// <returns>The receivers formatted as a string.</returns>
        /// <param name="divers">The <see cref="Diver"/>'s that should receive the mail.</param>
        string CreateReceiverString(ICollection<Diver> divers);
    }
}