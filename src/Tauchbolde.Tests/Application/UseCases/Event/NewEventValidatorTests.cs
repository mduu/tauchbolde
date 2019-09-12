using System;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Event.NewEventUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class NewEventValidatorTests
    {
        private readonly NewEventValidator validator = new NewEventValidator();
        private readonly DateTime validStartTime = new DateTime(2019, 9, 12, 19, 0, 0);

        [Fact]
        public void Validate_Success()
        {
            // Arrange
            NewEvent newEvent = CreateNewEvent();

            // Act
            var result = validator.Validate(newEvent);

            // Assert
            result.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public void Validate_EmptyCurrentUser_MustFail()
        {
            // Arrange
            NewEvent newEvent = CreateNewEvent(currentUserName: "");

            // Act
            var result = validator.Validate(newEvent);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(NewEvent.CurrentUserName));
        }


        [Fact]
        public void Validate_EndTimeBeforeStart_MustBeInvalid()
        {
            // Arrange
            var editEvent = CreateNewEvent(startTime: DateTime.Today, endTime: DateTime.Today.AddDays(-1));

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
            var editEvent = CreateNewEvent(startTime: DateTime.Today, endTime: DateTime.Today.AddHours(1));

            // Act
            var result = validator.Validate(editEvent);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_EmptyTitle_MustBeInvalid()
        {
            // Arrange
            var editEvent = CreateNewEvent(title: "");

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
            var editEvent = CreateNewEvent(location: "");

            // Act
            var result = validator.Validate(editEvent);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(editEvent.Location));
        }


        private NewEvent CreateNewEvent(
            string currentUserName = "john.doe",
            DateTime? startTime = null,
            DateTime? endTime = null,
            string title = "Test Event",
            string description = "desc",
            string location = "loc",
            string meetingPoint = "meet")
            =>
                new NewEvent(
                    currentUserName,
                    startTime ?? validStartTime,
                    endTime,
                    title,
                    location,
                    meetingPoint,
                    description);
    }
}