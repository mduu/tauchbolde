using FluentAssertions;
using Tauchbolde.InterfaceAdapters.UrlBuilders;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.UrlBuilders
{
    public class EmailUrlBuilderTests
    {
        [Fact]
        public void GetUrl_Success()
        {
            // Act
            var url = EmailUrlBuilder.GetUrl(DiverFactory.JohnDoeEmail);
            
            // Assert
            url.Should().Be($"mailto:{DiverFactory.JohnDoeEmail}");
        }
        
        [Fact]
        public void GetUrl_NullEmail_ReturnEmpty()
        {
            // Act
            var url = EmailUrlBuilder.GetUrl(null);
            
            // Assert
            url.Should().BeEmpty();
        }
        
        [Fact]
        public void GetUrl_EmptyEmail_ReturnEmpty()
        {
            // Act
            var url = EmailUrlBuilder.GetUrl("");
            
            // Assert
            url.Should().BeEmpty();
        }
    }
}