using System.IO;
using FluentAssertions;
using Tauchbolde.Commom.Misc;
using Tauchbolde.Common.Domain.PhotoStorage;
using Tauchbolde.Common.Domain.PhotoStorage.Stores.FileSystemStore;
using Xunit;

namespace Tauchbolde.Tests.Domain.PhotoStorage.Stores.FileSystemStore
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
        [InlineData("/root/path", PhotoCategory.Event, "image.jpg", null, 0, false, "/root/path/event/image.jpg")]
        [InlineData("/another_root/path", PhotoCategory.Event, "image.jpg", "", 0, false, "/another_root/path/event/image.jpg")]
        [InlineData("/root/path", PhotoCategory.Event, "image.jpg", "image/jpeg", 0, false, "/root/path/event/image.jpg")]
        [InlineData("/root/path", PhotoCategory.Event, "image", "image/jpeg", 0, false, "/root/path/event/image.jpg")]
        [InlineData("/root/path", PhotoCategory.Event, "image", "image/jpeg", 1, false, "/root/path/event/image_1.jpg")]
        [InlineData("/root/path", PhotoCategory.Event, "", "image/jpeg", 1, false, "/root/path/event/picture_1.jpg")]
        [InlineData("/root/path", PhotoCategory.Event, "image.jpg", null, 0, true, "/root/path/event/thumbs/image.jpg")]
        [InlineData("/root/path", PhotoCategory.Event, "IMAGE.JPG", null, 0, true, "/root/path/event/thumbs/IMAGE.JPG")]
        [InlineData("/root/path", PhotoCategory.Event, "IMAGE.JPG", null, 1, true, "/root/path/event/thumbs/IMAGE_1.JPG")]
        public void Calculate(
            string rootPath,
            PhotoCategory category,
            string baseFileName,
            string contentType,
            int count,
            bool isThumb,
            string expectedPath)
        {
            // Arrange
            rootPath = rootPath.Replace("/", Path.DirectorySeparatorChar.ToString());
            expectedPath = expectedPath.Replace("/", Path.DirectorySeparatorChar.ToString());

            // Act
            var filePath = pathCalculator.CalculatePath(
                rootPath,
                category,
                baseFileName,
                contentType,
                count,
                isThumb);

            // Assert
            filePath.Should().Be(expectedPath);
        }
        
        [Theory]
        [InlineData("IMG_A.JPG", false, "event/IMG_A.JPG")]
        [InlineData("IMG_B.JPG", false, "event/IMG_B_1.JPG")]
        [InlineData("IMG_C.JPG", false, "event/IMG_C_2.JPG")]
        [InlineData("IMG_C.JPG", true, "event/thumbs/IMG_C.JPG")]
        public void CalculateUniqueFilePath(string baseFileName, bool isThumb, string expectedFilePath)
        {
            // Arrange
            var rootPath = InitializeTestFileSystem();
            expectedFilePath = expectedFilePath.Replace("/", Path.DirectorySeparatorChar.ToString());
       
            // Act
            var filePath = pathCalculator.CalculateUniqueFilePath(
                rootPath,
                PhotoCategory.Event,
                baseFileName,
                "image/jpeg",
                isThumb);

            // Assert
            filePath.Should().Be(Path.Combine(rootPath, expectedFilePath));
            
            CleanPath(rootPath);
        }

        private string InitializeTestFileSystem()
        {
            var rootPath = Path.Combine(
                Path.GetTempPath(),
                nameof(FilePathCalculatorTests));

            Directory.CreateDirectory(rootPath);
            Directory.CreateDirectory(Path.Combine(rootPath, "event"));
            
            File.WriteAllText(Path.Combine(rootPath, "event", "IMG_B.JPG"), "This is a test-file");
            File.WriteAllText(Path.Combine(rootPath, "event", "IMG_C.JPG"), "This is a test-file");
            File.WriteAllText(Path.Combine(rootPath, "event", "IMG_C_1.JPG"), "This is a test-file");

            return rootPath;
        }

        private void CleanPath(string filePath)
        {
            Directory.Delete(filePath, true);
        }
    }
}