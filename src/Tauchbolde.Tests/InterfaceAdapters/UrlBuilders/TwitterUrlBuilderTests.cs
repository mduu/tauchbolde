using FluentAssertions;
using Tauchbolde.InterfaceAdapters.UrlBuilders;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.UrlBuilders
{
    public class TwitterUrlBuilderTests
    {
        [Theory]
        [InlineData("johndoe", "https://twitter.com/johndoe")]
        [InlineData("@johndoe", "https://twitter.com/johndoe")]
        [InlineData(null, "")]
        [InlineData("", "")]
        public void GetUrl(string twitterHandle, string expectedUrl)
        {
            // Act
            var url = TwitterUrlBuilder.GetUrl(twitterHandle);

            // Assert
            url.Should().Be(expectedUrl);
        }
    }
}