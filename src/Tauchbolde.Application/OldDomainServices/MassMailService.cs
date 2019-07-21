using System.Collections.Generic;
using System.Linq;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices
{
    internal class MassMailService : IMassMailService
    {
        public string CreateReceiverString(ICollection<Diver> divers)
            => string.Join(";",
                divers.Select(d => $"\"{d.Fullname}\"<{d.User.Email}>"));
    }
}
