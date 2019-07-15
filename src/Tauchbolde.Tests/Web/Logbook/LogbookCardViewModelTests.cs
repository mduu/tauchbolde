using System;
using FluentAssertions;
using Tauchbolde.Entities;
using Tauchbolde.Web.Models.ViewComponentModels;
using Xunit;

namespace Tauchbolde.Tests.Web.Logbook
{
    public class LogbookCardViewModelTests
    {        
        [Theory()]
        [InlineData("teaser", "text", "teaser")]
        [InlineData("", "text", "text")]
        [InlineData(null, "text", "text")]
        [InlineData("teaser", "", "teaser")]
        [InlineData("teaser", null, "teaser")]
        [InlineData("", "", "")]
        [InlineData("", null, null)]
        public void Test_TeaserText(string teaserText, string text, string expectedTeaser)
        {
            // Act
            var model = CreateInstance(teaserText, text);          
            
            // Assert
            model.TeaserText.Should().Be(expectedTeaser);
        }

        private LogbookCardViewModel CreateInstance(string teaser, string text)
            =>
                new LogbookCardViewModel(
                    new LogbookEntry
                    {
                        Id = new Guid("E6AC13DA-915F-43A6-9083-A0BAD0D13ED4"),
                        Title = "A title",
                        TeaserText = teaser,
                        Text = text
                    });
    }
}