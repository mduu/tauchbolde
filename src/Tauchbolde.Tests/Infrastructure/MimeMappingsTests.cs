using FluentAssertions;
using Tauchbolde.Commom.Misc;
using Xunit;

namespace Tauchbolde.Tests.Infrastructure
{
    public class MimeMappingsTests
    {
        private MimeMapping mimeMapping = new MimeMapping();
        
        [Theory]
        [InlineData("image/jpeg", ".jpg")]
        [InlineData("IMAGE/jpeg", ".jpg")]
        public void TestGetFileExtensionMapping(string mimeType, string expectedExtension)
        {
            // Act
            var extension = mimeMapping.GetFileExtensionMapping(mimeType);
            
            // Arrange
            extension.Should().Be(expectedExtension);
        }

        [Theory]
        [InlineData(".jpg", "image/jpeg")]
        [InlineData(".JPG", "image/jpeg")]
        [InlineData(".jpeg", "image/jpeg")]
        public void TestGetMimeMapping(string fileExtension, string expectedMimeType)
        {
            // Act
            var mimeType = mimeMapping.GetMimeMapping(fileExtension);
            
            // Arrange
            mimeType.Should().Be(expectedMimeType);
        }
    }
}