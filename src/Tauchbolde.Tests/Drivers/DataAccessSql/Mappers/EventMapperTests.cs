using System;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Newtonsoft.Json;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;
using Xunit;
using EventData = Tauchbolde.Driver.DataAccessSql.DataEntities.EventData;

namespace Tauchbolde.Tests.Drivers.DataAccessSql.Mappers
{
    [UseReporter(typeof(DiffReporter))]
    public class EventMapperTests
    {
        [Fact]
        public void Map()
        {
            var dataObject = new EventData
            {
                Id = new Guid("CF0CFA4E-DA42-4702-ACAA-5FCDC68E4FCE"),
                Name = "Test Event",
                Description = "A description",
                Canceled = true,
                Deleted = true,
                Location = "The location",
                MeetingPoint = "The Meeting-Point",
                OrganisatorId = new Guid("F82BE2F0-EE77-4C61-AFCF-8F604E133E0B"),
                StartTime = new DateTime(2019, 8, 1, 19, 0, 0),
                EndTime = new DateTime(2019, 8, 2, 12, 30, 0),
                Participants = Enumerable.Empty<ParticipantData>().ToList(),
                Comments = Enumerable.Empty<CommentData>().ToList(),
            };

            var domainObject = dataObject.MapTo();
            var newDataObject = domainObject.MapTo();
            
            newDataObject.Should().BeEquivalentTo(dataObject);
            Approvals.VerifyJson(JsonConvert.SerializeObject(
                new
                {
                    dataObject,
                    domainObject,
                    newDataObject
                }));
        }
    }
}