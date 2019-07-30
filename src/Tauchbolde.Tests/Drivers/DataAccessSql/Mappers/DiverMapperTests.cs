using System;
using System.Runtime.CompilerServices;
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
    public class DiverMapperTests
    {
        private readonly Guid diverId = new Guid("C35F8D49-057B-4A18-85D9-1AB3E866D3EC");

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Map()
        {
            var dataObject = new DiverData
            {
                Id = diverId,
                Education = "The Education",
                Experience = "The Experience",
                Slogan = "The Slogan",
                Fullname = "Fullname",
                Firstname = "Firstname",
                Lastname = "Lastname",
                UserId = "joe@doe.com",
                AvatarId = "TheAvatarId1234",
                FacebookId = "FacebookId1234",
                TwitterHandle = "TwitterHandle1234",
                SkypeId = "SkypeId1234",
                MemberSince = new DateTime(2017, 7, 1),
                MemberUntil = new DateTime(2019, 8, 13),
                MobilePhone = "4242 42 42",
                WebsiteUrl = "https://tauchbolde.ch",
                SendOwnNoticiations = true,
                LastNotificationCheckAt = new DateTime(2019, 8, 8, 13, 55, 00),
                NotificationIntervalInHours = 4,
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