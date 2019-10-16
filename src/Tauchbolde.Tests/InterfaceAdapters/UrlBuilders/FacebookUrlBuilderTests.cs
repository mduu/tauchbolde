using FluentAssertions;
using Tauchbolde.InterfaceAdapters.UrlBuilders;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.UrlBuilders
{
    public class FacebookUrlBuilderTests
    {
        [Theory]
        [InlineData("myFbId", "https://facebook.com/myFbId")]
        [InlineData(null, "")]
        [InlineData("", "")]
        public void GetUrl(string facebookId, string expectedUrl)
        {
            // Act
            var url = FacebookUrlBuilder.GetUrl(facebookId);
            
            // Assert
            url.Should().Be(expectedUrl);
        }
    }
}