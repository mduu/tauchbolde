using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;

namespace Tauchbolde.Driver.DataAccessSql.Mappers
{
    internal static class DiverMapper
    {
        public static Diver MapTo(this DiverData dataEntity) =>
            dataEntity == null
                ? null
                : new Diver
                {
                    Id = dataEntity.Id,
                    Firstname = dataEntity.Firstname,
                    Lastname = dataEntity.Lastname,
                    Fullname = dataEntity.Fullname,
                    Experience = dataEntity.Experience,
                    Education = dataEntity.Education,
                    MemberSince = dataEntity.MemberSince,
                    MemberUntil = dataEntity.MemberUntil,
                    Slogan = dataEntity.Slogan,
                    UserId = dataEntity.UserId,
                    User = dataEntity.User,
                    AvatarId = dataEntity.AvatarId,
                    FacebookId = dataEntity.FacebookId,
                    TwitterHandle = dataEntity.TwitterHandle,
                    SkypeId = dataEntity.SkypeId,
                    WebsiteUrl = dataEntity.WebsiteUrl,
                    MobilePhone = dataEntity.MobilePhone,
                    SendOwnNoticiations = dataEntity.SendOwnNoticiations,
                    LastNotificationCheckAt = dataEntity.LastNotificationCheckAt,
                    NotificationIntervalInHours = dataEntity.NotificationIntervalInHours
                };

        public static DiverData MapTo(this Diver domainEntity) =>
            domainEntity == null
                ? null
                : new DiverData
                {
                    Id = domainEntity.Id,
                    Firstname = domainEntity.Firstname,
                    Lastname = domainEntity.Lastname,
                    Fullname = domainEntity.Fullname,
                    Experience = domainEntity.Experience,
                    Education = domainEntity.Education,
                    MemberSince = domainEntity.MemberSince,
                    MemberUntil = domainEntity.MemberUntil,
                    Slogan = domainEntity.Slogan,
                    UserId = domainEntity.UserId,
                    User = domainEntity.User,
                    AvatarId = domainEntity.AvatarId,
                    FacebookId = domainEntity.FacebookId,
                    TwitterHandle = domainEntity.TwitterHandle,
                    SkypeId = domainEntity.SkypeId,
                    WebsiteUrl = domainEntity.WebsiteUrl,
                    MobilePhone = domainEntity.MobilePhone,
                    SendOwnNoticiations = domainEntity.SendOwnNoticiations,
                    LastNotificationCheckAt = domainEntity.LastNotificationCheckAt,
                    NotificationIntervalInHours = domainEntity.NotificationIntervalInHours,
                };
    }
}