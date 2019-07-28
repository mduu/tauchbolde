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
    public class NotificationMapperTests
    {
        [Fact]
        public void Map()
        {
            var dataObject = new NotificationData
            {
                Id = new Guid("674ED914-ACE3-4FF0-AD5C-4335CCF92607"),
                Message = "The Message",
               RecipientId = new Guid("2223CDFF-E5FE-4A68-81CD-BCC8800C365E"),
               Type = NotificationType.Neutral,
               EventId = new Guid("86E2735D-A537-43AD-9048-B1899A2E3F40"),
               AlreadySent = true,
               OccuredAt = new DateTime(2019, 8, 1),
               CountOfTries = 2,
               LogbookEntryId = new Guid("9BECCC85-BF62-4CAA-916E-3FB911868AE7"),
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