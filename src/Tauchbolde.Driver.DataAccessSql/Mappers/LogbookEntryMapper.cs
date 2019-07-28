using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;

namespace Tauchbolde.Driver.DataAccessSql.Mappers
{
    internal static class LogbookEntryMapper
    {
        public static LogbookEntry MapTo(this LogbookEntryData dataEntity) =>
            dataEntity == null
                ? null
                : new LogbookEntry
                {
                    Id = dataEntity.Id,
                    Title = dataEntity.Title,
                    TeaserText = dataEntity.TeaserText,
                    Text = dataEntity.Text,
                    TeaserImage = dataEntity.TeaserImage,
                    TeaserImageThumb = dataEntity.TeaserImageThumb,
                    CreatedAt = dataEntity.CreatedAt,
                    ModifiedAt = dataEntity.ModifiedAt,
                    IsFavorite = dataEntity.IsFavorite,
                    IsPublished = dataEntity.IsPublished,
                    ExternalPhotoAlbumUrl = dataEntity.ExternalPhotoAlbumUrl,
                    OriginalAuthorId = dataEntity.OriginalAuthorId,
                    OriginalAuthor = dataEntity.OriginalAuthor.MapTo(),
                    EventId = dataEntity.EventId,
                    EditorAuthorId = dataEntity.EditorAuthorId,
                };

        public static LogbookEntryData MapTo(this LogbookEntry domainEntity) =>
            domainEntity == null
                ? null
                : new LogbookEntryData
                {
                    Id = domainEntity.Id,
                    Title = domainEntity.Title,
                    TeaserText = domainEntity.TeaserText,
                    Text = domainEntity.Text,
                    TeaserImage = domainEntity.TeaserImage,
                    TeaserImageThumb = domainEntity.TeaserImageThumb,
                    CreatedAt = domainEntity.CreatedAt,
                    ModifiedAt = domainEntity.ModifiedAt,
                    IsFavorite = domainEntity.IsFavorite,
                    IsPublished = domainEntity.IsPublished,
                    ExternalPhotoAlbumUrl = domainEntity.ExternalPhotoAlbumUrl,
                    OriginalAuthorId = domainEntity.OriginalAuthorId,
                    OriginalAuthor = domainEntity.OriginalAuthor.MapTo(),
                    EventId = domainEntity.EventId,
                    EditorAuthorId = domainEntity.EditorAuthorId,
                };
    }
}