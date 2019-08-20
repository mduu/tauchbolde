using System;

namespace Tauchbolde.InterfaceAdapters
{
    public interface IRelativeUrlGenerator
    {
        string GenerateEventUrl(Guid? eventId);
        string GenerateLogbookEntryUrl(Guid? logbookEntryId);
    }
}