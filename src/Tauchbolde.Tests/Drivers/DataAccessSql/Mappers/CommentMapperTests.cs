using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Newtonsoft.Json;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;
using Xunit;

namespace Tauchbolde.Tests.Drivers.DataAccessSql.Mappers
{
    [UseReporter(typeof(DiffReporter))]
    public class CommentMapperTests
    {
        [Fact]
        public void Map()
        {
            var dataObject = new CommentData
            {
                Id = new Guid("DEBC03E1-FBFC-4895-AFBE-D8E221507C86"),
                AuthorId = new Guid("1C9B6622-2CAA-4FDF-9186-2331EB446616"),
                EventId = new Guid("925CE378-C86E-47EB-B98A-7D5689B5AEA6"),
                Text = "The Text!",
                CreateDate = new DateTime(2019, 12, 13),
                ModifiedDate = new DateTime(2019, 12, 14),
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