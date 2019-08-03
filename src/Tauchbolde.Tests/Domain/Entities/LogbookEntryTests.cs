using System;
using FluentAssertions;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.LogbookEntry;
using Tauchbolde.Domain.Types;
using Tauchbolde.Domain.ValueObjects;
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
            logbookEntry.UncommittedDomainEvents.Should().ContainSingle(e => e is LogbookEntryPublishedEvent);
        }
        
        [Fact]
        public void Publish_DontPublishAlreadyPublished()
        {
            var logbookEntry = new LogbookEntry { IsPublished = true };
            
            logbookEntry.Publish();

            logbookEntry.IsPublished.Should().BeTrue();
            logbookEntry.UncommittedDomainEvents.Should().BeEmpty();
        }
        
        [Fact]
        public void Unpublish_UnpublishPublished()
        {
            var logbookEntry = new LogbookEntry { IsPublished = true };
            
            logbookEntry.Unpublish();

            logbookEntry.IsPublished.Should().BeFalse();
            logbookEntry.UncommittedDomainEvents.Should().ContainSingle(e => e is LogbookEntryUnpublishedEvent);
        }
        
        [Fact]
        public void Unpublish_DontUnpublishedAlreadyUnpublished()
        {
            var logbookEntry = new LogbookEntry { IsPublished = false };
            
            logbookEntry.Unpublish();

            logbookEntry.IsPublished.Should().BeFalse();
            logbookEntry.UncommittedDomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void Edit_UpdateTeaserImage()
        {
            var logbookEntry = new LogbookEntry
            {
                Id = new Guid("601A3EC6-2BB7-4A47-83CF-389429824CEB"),
                Title = "Test Title",
                TeaserImage = "the_orig_image",
                TeaserImageThumb = "the_thumb_image",
            };
            var newOriginalPhotoIdentifier = new PhotoIdentifier(PhotoCategory.LogbookTeaser, false, "image.jpg");
            var newThumbnailPhotoIdentifier = new PhotoIdentifier(PhotoCategory.LogbookTeaser, true, "image.jpg");
            
            logbookEntry.Edit(
                "New Title",
                "New Teaser",
                "New Text",
                false,
                new Guid("4B3B79AE-3062-44FE-8AA1-14D425C081FA"),
                null,
                null,
                new PhotoAndThumbnailIdentifiers(
                    newOriginalPhotoIdentifier, 
                    newThumbnailPhotoIdentifier)
                );

            logbookEntry.TeaserImage.Should().Be(newOriginalPhotoIdentifier.Serialze());
            logbookEntry.TeaserImageThumb.Should().Be(newThumbnailPhotoIdentifier.Serialze());
        }
    }
}