using System.Collections.Generic;
using System.Linq;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices
{
    internal class MassMailService : IMassMailService
    {
        /// <inheritdoc/>
        public string CreateReceiverString(ICollection<Diver> divers)
            => string.Join(";",
                divers.Select(d => $"\"{d.Fullname}\"<{d.User.Email}>"));
    }
}
