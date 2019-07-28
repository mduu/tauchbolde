using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Newtonsoft.Json;
using Tauchbolde.Domain.Types;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;
using Xunit;

namespace Tauchbolde.Tests.Drivers.DataAccessSql.Mappers
{
    [UseReporter(typeof(DiffReporter))]
    public class ParticipantMapperTests
    {
        [Fact]
        public void Map()
        {
            var dataObject = new ParticipantData
            {
                Id = new Guid("BB8E0FCE-974A-4877-80A9-5E4CA315E166"),
                Status = ParticipantStatus.Tentative,
                Note = "The Note",
                CountPeople = 2,
                EventId = new Guid("C2847D0C-CAFA-4ED8-9496-10039AA6A6F5"),
                ParticipatingDiverId = new Guid("1549F94E-E376-4AC3-9591-C6938A1C9421"),
                BuddyTeamName = "Team 1",
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