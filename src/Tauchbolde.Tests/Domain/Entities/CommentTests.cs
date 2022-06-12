using FluentAssertions;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.SharedKernel.Services;
using Xunit;

namespace Tauchbolde.Tests.Domain.Entities
{
    public class CommentTests
    {
        private readonly Guid validCommentId = new Guid("2E8588E4-A9B8-457A-BD95-1F4D5C6791E6");
        private readonly Guid validEventId = new Guid("5CE5A334-4501-4DF8-A88B-7254A3283D44");
        private readonly Guid validAuthorId = new Guid("262B645E-B4B8-4881-8745-43759B1F278D");

        [Fact]
        public void Edit_Success()
        {
            // Arrange
            var createdTime = new DateTime(2019, 8, 22, 8, 0, 0);
            var modifiedTime = new DateTime(2019, 8, 22, 9, 0, 0);
            var comment = new Comment
            {
                Id = validCommentId,
                AuthorId = validAuthorId,
                EventId = validEventId,
                CreateDate = createdTime,
                Text = "a comment",
            };
            SystemClock.SetTime(modifiedTime);

            // Act
            comment.Edit("a new text");

            // Assert
            comment.Id.Should().Be(validCommentId);
            comment.AuthorId.Should().Be(validAuthorId);
            comment.EventId.Should().Be(validEventId);
            comment.Text.Should().Be("a new text");
            comment.ModifiedDate.Should().Be(modifiedTime);
            comment.UncommittedDomainEvents.Should().ContainSingle(e =>
                ((CommentEditedEvent) e).Text == "a new text" &&
                ((CommentEditedEvent) e).CommentId == validCommentId);
        }
    }
}