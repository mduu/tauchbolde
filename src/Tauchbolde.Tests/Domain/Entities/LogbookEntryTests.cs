using FluentAssertions;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events;
using Xunit;

namespace Tauchbolde.Tests.Domain.Entities
{
    public class LogbookEntryTests
    {
        [Fact]
        public void Publish_PublishUnpublished()
        {
            var logbookEntry = new LogbookEntry { IsPublished = false };
            
            logbookEntry.Publish();

            logbookEntry.IsPublished.Should().BeTrue();
            logbookEntry.Events.Should().ContainSingle(e => e is LogbookEntryPublishedEvent);
        }
        
        [Fact]
        public void Publish_DontPublishAlreadyPublished()
        {
            var logbookEntry = new LogbookEntry { IsPublished = true };
            
            logbookEntry.Publish();

            logbookEntry.IsPublished.Should().BeTrue();
            logbookEntry.Events.Should().BeEmpty();
        }
        
        [Fact]
        public void Unpublish_UnpublishPublished()
        {
            var logbookEntry = new LogbookEntry { IsPublished = true };
            
            logbookEntry.Unpublish();

            logbookEntry.IsPublished.Should().BeFalse();
            logbookEntry.Events.Should().ContainSingle(e => e is LogbookEntryUnpublishedEvent);
        }
        
        [Fact]
        public void Unpublish_DontUnpublishedAlreadyUnpublished()
        {
            var logbookEntry = new LogbookEntry { IsPublished = false };
            
            logbookEntry.Unpublish();

            logbookEntry.IsPublished.Should().BeFalse();
            logbookEntry.Events.Should().BeEmpty();
        }
    }
}