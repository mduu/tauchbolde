using FluentAssertions;
using Tauchbolde.Application.UseCases.Event.EditEventUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class EditEventValidatorTests
    {
        private readonly EditEventValidator validator = new();

        [Fact]
        public void Validate_Success()
        {
            // Arrange
            var editEvent = CreateEditEvent();
            
            // Act
            var result = validator.Validate(editEvent);
            
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_EmptyEventId_MustBeInvalid()
        {
            // Arrange
            var editEvent = CreateEditEvent(startTime: DateTime.Today, eventId: Guid.Empty);

            // Act
            var result = validator.Validate(editEvent);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(editEvent.EventId));
        }

        [Fact]
        public void Validate_EndTimeBeforeStart_MustBeInvalid()
        {
            // Arrange
            var editEvent = CreateEditEvent(startTime: DateTime.Today, endTime: DateTime.Today.AddDays(-1));

            // Act
            var result = validator.Validate(editEvent);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(editEvent.EndTime));
        }

        [Fact]
        public void Validate_EndTimeAfterStart_MustBeInvalid()
        {
            // Arrange
            var editEvent = CreateEditEvent(startTime: DateTime.Today, endTime: DateTime.Today.AddHours(1));

            // Act
            var result = validator.Validate(editEvent);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_EmptyTitle_MustBeInvalid()
        {
            // Arrange
            var editEvent = CreateEditEvent(title: "");

            // Act
            var result = validator.Validate(editEvent);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(editEvent.Title));
        }

        [Fact]
        public void Validate_EmptyLocation_MustBeInvalid()
        {
            // Arrange
            var editEvent = CreateEditEvent(location: "");

            // Act
            var result = validator.Validate(editEvent);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(editEvent.Location));
        }
        
        private static EditEvent CreateEditEvent(DateTime? startTime = null,
            string description = "",
            string meetingPoint = "",
            string location = "Some Place",
            string title = "Test Event",
            DateTime? endTime = null,
            Guid? eventId = null) =>
            new(
                eventId ?? new Guid("E68A9C75-A4D8-43D8-8052-CFD400DB52E5"),
                startTime ?? DateTime.Today,
                endTime,
                title,
                location,
                meetingPoint,
                description);
    }
}