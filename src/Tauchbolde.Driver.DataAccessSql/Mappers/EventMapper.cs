using System.Linq;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using EventData = Tauchbolde.Driver.DataAccessSql.DataEntities.EventData;

namespace Tauchbolde.Driver.DataAccessSql.Mappers
{
    internal static class EventMapper
    {
        public static Event MapTo(this EventData dataEntity) =>
            dataEntity == null
                ? null
                : new Event
                {
                    Id = dataEntity.Id,
                    Name = dataEntity.Name,
                    Description = dataEntity.Description,
                    Deleted = dataEntity.Deleted,
                    MeetingPoint = dataEntity.MeetingPoint,
                    Location = dataEntity.Location,
                    StartTime = dataEntity.StartTime,
                    EndTime = dataEntity.EndTime,
                    Canceled = dataEntity.Canceled,
                    OrganisatorId = dataEntity.OrganisatorId,
                    Organisator = dataEntity.Organisator.MapTo(),
                    Comments = 
                        (dataEntity.Comments?.Select(c => c.MapTo()) ?? Enumerable.Empty<Comment>())
                        .ToList(),
                    Participants =
                        (dataEntity.Participants?.Select(p => p.MapTo()) ?? Enumerable.Empty<Participant>())
                        .ToList(),
                };

        public static EventData MapTo(this Event domainEntity) =>
            domainEntity == null
                ? null
                : new EventData
                {
                    Id = domainEntity.Id,
                    Name = domainEntity.Name,
                    Description = domainEntity.Description,
                    Deleted = domainEntity.Deleted,
                    MeetingPoint = domainEntity.MeetingPoint,
                    Location = domainEntity.Location,
                    Canceled = domainEntity.Canceled,
                    StartTime = domainEntity.StartTime,
                    EndTime = domainEntity.EndTime,
                    OrganisatorId = domainEntity.OrganisatorId,
                    Organisator = domainEntity.Organisator.MapTo(),
                    Comments = 
                        (domainEntity.Comments?.Select(c => c.MapTo()) ?? Enumerable.Empty<CommentData>())
                        .ToList(),
                    Participants = 
                        (domainEntity.Participants?.Select(p => p.MapTo()) ?? Enumerable.Empty<ParticipantData>())
                        .ToList(),
                };
    }
}