using FluentAssertions;
using Tauchbolde.InterfaceAdapters.UrlBuilders;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.UrlBuilders
{
    public class PhoneUrlBuilderTests
    {
        [Theory]
        [InlineData("+41 745 50 60", "tel:+41 745 50 60")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void GetUrl(string phoneNumber, string expectedUrl)
        {
            // Act
            var url = PhoneUrlBuilder.GetUrl(phoneNumber);

            // Assert
            url.Should().Be(expectedUrl);
        }
    }
}