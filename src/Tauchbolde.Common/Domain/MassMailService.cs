using System.Collections.Generic;
using System.Linq;
using Tauchbolde.Entities;

namespace Tauchbolde.Common.Domain
{
    internal class MassMailService : IMassMailService
    {
        /// <inheritdoc/>
        public string CreateReceiverString(ICollection<Diver> divers)
            => string.Join(";",
                divers.Select(d => $"\"{d.Fullname}\"<{d.User.Email}>"));
    }
}
