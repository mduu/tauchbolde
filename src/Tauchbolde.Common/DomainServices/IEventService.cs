using System.IO;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices
{
    public interface IEventService
    {
        Stream CreateIcalForEvent(Event evt);
    }
}