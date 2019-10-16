using FluentAssertions;
using Tauchbolde.InterfaceAdapters.UrlBuilders;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.UrlBuilders
{
    public class SkypeUrlBuilderTests
    {
        [Theory]
        [InlineData("my_skype_id", "skype:my_skype_id")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void GetUrl(string skypeId, string expectedUrl)
        {
            // Act
            var url = SkypeUrlBuilder.GetUrl(skypeId);

            // Assert
            url.Should().Be(expectedUrl);
        }
    }
}