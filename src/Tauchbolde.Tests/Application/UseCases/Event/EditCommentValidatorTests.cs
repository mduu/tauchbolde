using System;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Event.EditCommentUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class EditCommentValidatorTests
    {
        private readonly EditCommentValidator validator = new();

        [Fact]
        public void Validate_Success()
        {
            // Arrange
            var instance = new EditComment(new Guid("E256C182-B487-4E81-B639-F9416B15A4C0"), "a comment");
 
            // Act
            var result = validator.Validate(instance);
            
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_EmptyTextMustFail()
        {
            // Arrange
            var instance = new EditComment(new Guid("E256C182-B487-4E81-B639-F9416B15A4C0"), "");
 
            // Act
            var result = validator.Validate(instance);
            
            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_EmptyCommentIdMustFail()
        {
            // Arrange
            var instance = new EditComment(Guid.Empty, "a comment");
 
            // Act
            var result = validator.Validate(instance);
            
            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}