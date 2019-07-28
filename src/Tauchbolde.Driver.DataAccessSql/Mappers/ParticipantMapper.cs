using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;

namespace Tauchbolde.Driver.DataAccessSql.Mappers
{
    internal static class ParticipantMapper
    {
        public static Participant MapTo(this ParticipantData dataEntity) =>
            dataEntity == null
                ? null
                : new Participant
                {
                    Id = dataEntity.Id,
                    Note = dataEntity.Note,
                    Status = dataEntity.Status,
                    CountPeople = dataEntity.CountPeople,
                    EventId = dataEntity.EventId,
                    Event = dataEntity.Event.MapTo(),
                    BuddyTeamName = dataEntity.BuddyTeamName,
                    ParticipatingDiverId = dataEntity.ParticipatingDiverId,
                    ParticipatingDiver = dataEntity.ParticipatingDiver.MapTo(),
                };

        public static ParticipantData MapTo(this Participant domainEntity) =>
            domainEntity == null
                ? null
                : new ParticipantData
                {
                    Id = domainEntity.Id,
                    Note = domainEntity.Note,
                    Status = domainEntity.Status,
                    CountPeople = domainEntity.CountPeople,
                    EventId = domainEntity.EventId,
                    Event = domainEntity.Event.MapTo(),
                    BuddyTeamName = domainEntity.BuddyTeamName,
                    ParticipatingDiverId = domainEntity.ParticipatingDiverId,
                    ParticipatingDiver = domainEntity.ParticipatingDiver.MapTo(),
                };
    }
}