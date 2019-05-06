using System.IO;
using FluentAssertions;
using Tauchbolde.Commom.Misc;
using Tauchbolde.Common.DomainServices.PhotoStorage;
using Tauchbolde.Common.DomainServices.PhotoStorage.Stores.FileSystemStore;
using Xunit;

namespace Tauchbolde.Tests.DomainServices.PhotoStorage.Stores.FileSystemStore
{
    public class FilePathCalculatorTests
    {
        private readonly MimeMapping mapping = new MimeMapping();
        private readonly FilePathCalculator pathCalculator;

        public FilePathCalculatorTests()
        {
            pathCalculator = new FilePathCalculator(mapping);
        }

        [Theory]
        [InlineData("/root/path", PhotoCategory.EventTeaser, "image.jpg", null, 0, ThumbnailType.None, "/root/path/eventteaser/image.jpg")]
        [InlineData("/another_root/path", PhotoCategory.EventTeaser, "image.jpg", "", 0, ThumbnailType.None, "/another_root/path/eventteaser/image.jpg")]
        [InlineData("/root/path", PhotoCategory.EventTeaser, "image.jpg", "image/jpeg", 0, ThumbnailType.None, "/root/path/eventteaser/image.jpg")]
        [InlineData("/root/path", PhotoCategory.EventTeaser, "image", "image/jpeg", 0, ThumbnailType.None, "/root/path/eventteaser/image.jpg")]
        [InlineData("/root/path", PhotoCategory.EventTeaser, "image", "image/jpeg", 1, ThumbnailType.None, "/root/path/eventteaser/image_1.jpg")]
        [InlineData("/root/path", PhotoCategory.EventTeaser, "", "image/jpeg", 1, ThumbnailType.None, "/root/path/eventteaser/picture_1.jpg")]
        [InlineData("/root/path", PhotoCategory.EventTeaser, "image.jpg", null, 0, ThumbnailType.LogbookTeaser, "/root/path/eventteaser/thumbs/image.jpg")]
        [InlineData("/root/path", PhotoCategory.EventTeaser, "IMAGE.JPG", null, 0, ThumbnailType.LogbookTeaser, "/root/path/eventteaser/thumbs/IMAGE.JPG")]
        [InlineData("/root/path", PhotoCategory.EventTeaser, "IMAGE.JPG", null, 1, ThumbnailType.LogbookTeaser, "/root/path/eventteaser/thumbs/IMAGE_1.JPG")]
        public void Calculate(
            string rootPath,
            PhotoCategory category,
            string baseFileName,
            string contentType,
            int count,
            ThumbnailType thumbnailType,
            string expectedPath)
        {
            // Arrange

            // Act
            var filePath = pathCalculator.CalculatePath(
                rootPath,
                category,
                baseFileName,
                contentType,
                count,
                thumbnailType);

            // Assert
            filePath.Should().Be(expectedPath);
        }
        
        [Theory]
        [InlineData("IMG_A.JPG", ThumbnailType.None, "/eventteaser/IMG_A.JPG")]
        [InlineData("IMG_B.JPG", ThumbnailType.None, "/eventteaser/IMG_B_1.JPG")]
        [InlineData("IMG_C.JPG", ThumbnailType.None, "/eventteaser/IMG_C_2.JPG")]
        [InlineData("IMG_C.JPG", ThumbnailType.LogbookTeaser, "/eventteaser/thumbs/IMG_C.JPG")]
        public void CalculateUniqueFilePath(string baseFileName, ThumbnailType thumbnailType, string expectedFilePath)
        {
            // Arrange
            var rootPath = InitializeTestFileSystem();
            
            // Act
            var filePath = pathCalculator.CalculateUniqueFilePath(
                rootPath,
                PhotoCategory.EventTeaser,
                baseFileName,
                "image/jpeg",
                thumbnailType);

            // Assert
            filePath.Should().Be($"{rootPath}{expectedFilePath}");
            
            CleanPath(rootPath);
        }

        private string InitializeTestFileSystem()
        {
            var rootPath = Path.Combine(
                Path.GetTempPath(),
                nameof(FilePathCalculatorTests));

            Directory.CreateDirectory(rootPath);
            Directory.CreateDirectory(Path.Combine(rootPath, "eventteaser"));
            
            File.WriteAllText(Path.Combine(rootPath, "eventteaser", "IMG_B.JPG"), "This is a test-file");
            File.WriteAllText(Path.Combine(rootPath, "eventteaser", "IMG_C.JPG"), "This is a test-file");
            File.WriteAllText(Path.Combine(rootPath, "eventteaser", "IMG_C_1.JPG"), "This is a test-file");

            return rootPath;
        }

        private void CleanPath(string filePath)
        {
            Directory.Delete(filePath, true);
        }
    }
}