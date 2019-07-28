using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;

namespace Tauchbolde.Driver.DataAccessSql.Mappers
{
    internal static class NotificationMapper
    {
        public static Notification MapTo(this NotificationData dataEntity) =>
            dataEntity == null
                ? null
                : new Notification
                {
                    Id = dataEntity.Id,
                    RecipientId = dataEntity.RecipientId,
                    Recipient = dataEntity.Recipient.MapTo(),
                    Message = dataEntity.Message,
                    Type = dataEntity.Type,
                    AlreadySent = dataEntity.AlreadySent,
                    OccuredAt = dataEntity.OccuredAt,
                    CountOfTries = dataEntity.CountOfTries,
                    EventId = dataEntity.EventId,
                    Event = dataEntity.Event.MapTo(),
                    LogbookEntryId = dataEntity.LogbookEntryId,
                    LogbookEntry = dataEntity.LogbookEntry.MapTo(),
                };

        public static NotificationData MapTo(this Notification domainEntity) =>
            domainEntity == null
                ? null
                : new NotificationData
                {
                    Id = domainEntity.Id,
                    RecipientId = domainEntity.RecipientId,
                    Recipient = domainEntity.Recipient.MapTo(),
                    Message = domainEntity.Message,
                    Type = domainEntity.Type,
                    AlreadySent = domainEntity.AlreadySent,
                    OccuredAt = domainEntity.OccuredAt,
                    CountOfTries = domainEntity.CountOfTries,
                    EventId = domainEntity.EventId,
                    LogbookEntryId = domainEntity.LogbookEntryId,
                };
    }
}