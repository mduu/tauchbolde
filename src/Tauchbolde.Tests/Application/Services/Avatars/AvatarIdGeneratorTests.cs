using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Tauchbolde.Application.Services.Avatars;
using Xunit;

namespace Tauchbolde.Tests.Application.Services.Avatars
{
    public class AvatarIdGeneratorTests
    {
        [Theory]
        [InlineData("john", "jpg", "john_1.jpg")]
        [InlineData("john", "png", "john_1.png")]
        [InlineData("JOHN", "png", "JOHN_1.png")]
        [InlineData("jane", "png", "jane_2.png")]
        [InlineData("jane", "jpg", "jane_2.jpg")]
        [InlineData("JANE", "jpg", "JANE_2.jpg")]
        [InlineData("mike", "jpg", "mike_1.jpg")]
        [InlineData("MIKE", "JPG", "MIKE_1.JPG")]
        public void TestGenerate(string firstName, string fileExt, string expectedAvatarId)
        {
            // Arrange
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var rootPath = Path.GetDirectoryName(codeBasePath);
            var avatarPath = Path.Combine(rootPath, "avatar");
            Directory.CreateDirectory(avatarPath);
            
            File.WriteAllBytes(Path.Combine(avatarPath, "jane_1.png"), new byte[] { 0x00 });
            File.WriteAllBytes(Path.Combine(avatarPath, "mike_2.png"), new byte[] { 0x00 });
            var pathProvider = new AvatarPathProviderMock(rootPath);
            var generator = new AvatarIdGenerator(pathProvider);

            // Act
            var newAvatarId = generator.Generate(firstName, fileExt);

            // Assert
            newAvatarId.Should().Be(expectedAvatarId);

            // Clean-Up
            try
            {
                Directory.Delete(avatarPath, true);
            }
            catch (IOException) {}
        }
    }
}
