using System;
using FluentAssertions;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.SharedKernel.Services;
using Xunit;

namespace Tauchbolde.Tests.Domain.Entities
{
    public class EventTests
    {
        private readonly Guid validEventId = new Guid("D71FA752-2663-4197-831D-F241585F5CD5");

        [Fact]
        public void AddNewComment_Success()
        {
            var currentTime = new DateTime(2019, 8, 21, 8, 0, 0, 0);
            SystemClock.SetTime(currentTime);
            var evt = new Event { Id = validEventId };
            var authorId = new Guid("F39AB6D9-7374-4481-AD66-5946FDBBDA0A");
            var text = "Test comment!";
            
            var newComment = evt.AddNewComment(authorId, text);

            newComment.Should().NotBeNull();
            newComment.Id.Should().NotBeEmpty();
            newComment.AuthorId.Should().Be(authorId);
            newComment.EventId.Should().Be(evt.Id);
            newComment.Text.Should().Be(text);
            newComment.CreateDate.Should().Be(currentTime);
            newComment.ModifiedDate.Should().BeNull();
            evt.Comments.Should().ContainSingle(c => c.Id == newComment.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AddNewComment_FailWithEmptyText(string text)
        {
            var evt = new Event { Id = validEventId };
            var authorId = new Guid("F39AB6D9-7374-4481-AD66-5946FDBBDA0A");
            
            Func<Comment> act = () => evt.AddNewComment(authorId, text);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Edit_Success()
        {
            // Arrange
            var organisatorId = new Guid("62C42B1F-B457-4449-B326-7FD9AAB50EB3");
            var evt = new Event
            {
                Id = validEventId,
                OrganisatorId = organisatorId
            };
            var startTime = new DateTime(2019, 9, 10, 22, 56, 0);
            var endTime = new DateTime(2019, 9, 10, 23, 30, 0);
      
            // Act
            var result = evt.Edit(
                organisatorId,
                "The Title",
                "The Description",
                "location",
                "meetingpoint",
                startTime,
                endTime);

            // Assert
            result.Should().BeTrue();
            evt.Name.Should().Be("The Title");
            evt.Description.Should().Be("The Description");
            evt.Location.Should().Be("location");
            evt.MeetingPoint.Should().Be("meetingpoint");
            evt.StartTime.Should().Be(startTime);
            evt.EndTime.Should().Be(endTime);
            evt.UncommittedDomainEvents.Should().ContainSingle(e => e.GetType() == typeof(EventEditedEvent));
        }

        [Fact]
        public void Edit_NotOrganisator_MustFail()
        {
            // Arrange
            var organisatorId = new Guid("62C42B1F-B457-4449-B326-7FD9AAB50EB3");
            var evt = new Event
            {
                Id = validEventId,
                OrganisatorId = organisatorId
            };
            
            // Act
            var result = evt.Edit(
                new Guid("D0D59385-CE0A-465E-80E8-74B0619D921F"), 
                "The Title",
                "The Description",
                "location",
                "meetingpoint",
                new DateTime(2019, 9, 10, 22, 56, 0),
                new DateTime(2019, 9, 10, 23, 30, 0));

            // Assert
            result.Should().BeFalse();
        }
    }
}