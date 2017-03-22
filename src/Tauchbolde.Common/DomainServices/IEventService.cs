using System;
using System.IO;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public interface IEventService
    {
        Task<Stream> CreateIcalForEvent(Guid eventId, IEventRepository eventRepository);
    }
}