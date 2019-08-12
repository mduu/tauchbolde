using System;
using FluentAssertions;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Domain.Entities
{
    public class EventTests
    {
        private readonly Guid validEventId = new Guid("D71FA752-2663-4197-831D-F241585F5CD5");

        [Fact]
        public void AddNewComment_Success()
        {
            var evt = new Event { Id = validEventId };
            var authorId = new Guid("F39AB6D9-7374-4481-AD66-5946FDBBDA0A");
            var text = "Test comment!";
            var now = DateTime.UtcNow;
            
            var newComment = evt.AddNewComment(authorId, text);

            newComment.Should().NotBeNull();
            newComment.Id.Should().NotBeEmpty();
            newComment.AuthorId.Should().Be(authorId);
            newComment.EventId.Should().Be(evt.Id);
            newComment.Text.Should().Be(text);
            newComment.CreateDate.Should().BeAfter(now);
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
    }
}