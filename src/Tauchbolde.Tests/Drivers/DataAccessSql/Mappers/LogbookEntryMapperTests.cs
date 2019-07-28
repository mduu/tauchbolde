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
    public class LogbookEntryMapperTests
    {
        [Fact]
        public void Map()
        {
            var dataObject = new LogbookEntryData
            {
                Id = new Guid("08F81971-81C9-4ED8-932D-95940B6E27A3"),
                Title = "The Title",
                TeaserText = "The Teaser",
                Text = "The Text",
                CreatedAt = new DateTime(2019, 8, 1, 12, 0, 0),
                ModifiedAt = new DateTime(2019, 8, 2, 13, 0, 0),
                IsFavorite = true,
                IsPublished = true,
                EventId = new Guid("6D3CD14C-15AF-4BAE-8CC1-A0B6A9EE397D"),
                TeaserImage = "TheTeaserImage",
                TeaserImageThumb = "TheTeaserImageThumb",
                OriginalAuthorId = new Guid("BFBEA88B-3A40-4487-9FC4-B477DD20E17C"),
                EditorAuthorId = new Guid("5A835146-69F7-4A19-BE1B-E7501D01BD8D"),
                ExternalPhotoAlbumUrl = "https://tauchbolde.ch",
            };

            var domainObject = dataObject.MapTo();
            var newDataObject = domainObject.MapTo();
            
            newDataObject.Should().BeEquivalentTo(dataObject);
            Approvals.VerifyJson(JsonConvert.SerializeObject(new
            {
                dataObject,
                domainObject,
                newDataObject
            }));
        }
    }
}