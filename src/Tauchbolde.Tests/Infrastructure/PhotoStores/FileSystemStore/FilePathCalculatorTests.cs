using System.IO;
using FluentAssertions;
using Tauchbolde.Application.Services;
using Tauchbolde.Domain.Types;
using Tauchbolde.Driver.PhotoStorage.FileSystemStore;
using Xunit;

namespace Tauchbolde.Tests.Infrastructure.PhotoStores.FileSystemStore
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
        [InlineData("/root/path", PhotoCategory.LogbookTeaser, "image.jpg", null, 0, false, "/root/path/logbookteaser/image.jpg")]
        [InlineData("/another_root/path", PhotoCategory.LogbookTeaser, "image.jpg", "", 0, false, "/another_root/path/logbookteaser/image.jpg")]
        [InlineData("/root/path", PhotoCategory.LogbookTeaser, "image.jpg", "image/jpeg", 0, false, "/root/path/logbookteaser/image.jpg")]
        [InlineData("/root/path", PhotoCategory.LogbookTeaser, "image", "image/jpeg", 0, false, "/root/path/logbookteaser/image.jpg")]
        [InlineData("/root/path", PhotoCategory.LogbookTeaser, "image", "image/jpeg", 1, false, "/root/path/logbookteaser/image_1.jpg")]
        [InlineData("/root/path", PhotoCategory.LogbookTeaser, "", "image/jpeg", 1, false, "/root/path/logbookteaser/picture_1.jpg")]
        [InlineData("/root/path", PhotoCategory.LogbookTeaser, "image.jpg", null, 0, true, "/root/path/logbookteaser/thumbs/image.jpg")]
        [InlineData("/root/path", PhotoCategory.LogbookTeaser, "IMAGE.JPG", null, 0, true, "/root/path/logbookteaser/thumbs/IMAGE.JPG")]
        [InlineData("/root/path", PhotoCategory.LogbookTeaser, "IMAGE.JPG", null, 1, true, "/root/path/logbookteaser/thumbs/IMAGE_1.JPG")]
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
        [InlineData("IMG_A.JPG", false, "logbookteaser/IMG_A.JPG")]
        [InlineData("IMG_B.JPG", false, "logbookteaser/IMG_B_1.JPG")]
        [InlineData("IMG_C.JPG", false, "logbookteaser/IMG_C_2.JPG")]
        [InlineData("IMG_C.JPG", true, "logbookteaser/thumbs/IMG_C.JPG")]
        public void CalculateUniqueFilePath(string baseFileName, bool isThumb, string expectedFilePath)
        {
            // Arrange
            var rootPath = InitializeTestFileSystem();
            expectedFilePath = expectedFilePath.Replace("/", Path.DirectorySeparatorChar.ToString());
       
            // Act
            var filePath = pathCalculator.CalculateUniqueFilePath(
                rootPath,
                PhotoCategory.LogbookTeaser,
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
            Directory.CreateDirectory(Path.Combine(rootPath, "logbookteaser"));
            
            File.WriteAllText(Path.Combine(rootPath, "logbookteaser", "IMG_B.JPG"), "This is a test-file");
            File.WriteAllText(Path.Combine(rootPath, "logbookteaser", "IMG_C.JPG"), "This is a test-file");
            File.WriteAllText(Path.Combine(rootPath, "logbookteaser", "IMG_C_1.JPG"), "This is a test-file");

            return rootPath;
        }

        private void CleanPath(string filePath)
        {
            Directory.Delete(filePath, true);
        }
    }
}