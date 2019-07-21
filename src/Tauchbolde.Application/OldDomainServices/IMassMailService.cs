using System.Collections.Generic;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices
{
    public interface IMassMailService
    {
        string CreateReceiverString(ICollection<Diver> divers);
    }
}