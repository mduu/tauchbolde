using System;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class GetEventDetailsValidatorTests
    {
        private readonly GetEventDetailsValidator validator = new GetEventDetailsValidator();

        [Fact]
        public void Validate_Success()
        {
            // Arrange
            var obj = new GetEventDetails(
                new Guid("06001232-6A1B-4F2D-AD15-80DA03A4E5E6"),
                A.Fake<IEventDetailsPresenter>());
            
            // Act
            var result = validator.Validate(obj);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_EmptyEventId_MustFail()
        {
            // Arrange
            var obj = new GetEventDetails(
                Guid.Empty,
                A.Fake<IEventDetailsPresenter>());
            
            // Act
            var result = validator.Validate(obj);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(obj.EventId));
        }

        [Fact]
        public void Validate_NullPresenter_MustFail()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new GetEventDetails(
                new Guid("06001232-6A1B-4F2D-AD15-80DA03A4E5E6"),
                null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("presenter");
        }
    }
}