using System;
using FluentAssertions;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.Diver;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Domain.Entities
{
    public class DiverTests
    {
        [Fact]
        public void Edit_Success()
        {
            // Arrange
            var diver = DiverFactory.CreateJohnDoe();

            // Act
            diver.Edit(
                DiverFactory.JaneDoeDiverId,
                "John Doe New",
                "John New",
                "Doe New",
                "New Skill Level",
                "3000 Dives",
                "New Slogan",
                "+41 999 88 77",
                "https://john.com",
                "joFb",
                "doe",
                "doe.john");

            // Assert
            diver.UncommittedDomainEvents.Should().ContainSingle(e =>
                e.GetType() == typeof(UserProfileEditedEvent) &&
                ((UserProfileEditedEvent) e).EditedDiverId == DiverFactory.JohnDoeDiverId &&
                ((UserProfileEditedEvent) e).EditedByDiverId == DiverFactory.JaneDoeDiverId);
        }
    }
}