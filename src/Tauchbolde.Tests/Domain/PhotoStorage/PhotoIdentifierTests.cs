using FluentAssertions;
using Tauchbolde.Common.Domain.PhotoStorage;
using Xunit;

namespace Tauchbolde.Tests.Domain.PhotoStorage
{
    public class PhotoIdentifierTests
    {
        [Fact]
        public void TestSerializeOriginalPhotoIdentifier()
        {
            var result = new PhotoIdentifier("LogbookTeaser-myimage.jpg");

            result.Category.Should().Be(PhotoCategory.LogbookTeaser);
            result.IsThumb.Should().BeFalse();
            result.Filename.Should().Be("myimage.jpg");
        }
        
        [Fact]
        public void TestSerializeThumbPhotoIdentifier()
        {
            var result = new PhotoIdentifier("LogbookTeaser-thumbs-myimage.jpg");

            result.Category.Should().Be(PhotoCategory.LogbookTeaser);
            result.IsThumb.Should().BeTrue();
            result.Filename.Should().Be("myimage.jpg");
        }
    }
}