namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Logbook.Details
{
    public interface ILogbookDetailsUrlGenerator
    {
        string GenerateEditUrl(Guid logbookEntryId);
        string GeneratePublishUrl(Guid logbookEntryId);
        string GenerateUnPublishUrl(Guid logbookEntryId);
        string GenerateDeleteUrl(Guid logbookEntryId);
        string GenerateTeaserImageUrl(string teaserImageIdentifier);
    }
}