using System;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.Diver;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Domain.Entities
{
    public class DiverTests
    {
        [Fact]
        public void Ctor_Success()
        {
            // Arrange
            var user = new IdentityUser(DiverFactory.JohnDoeEmail)
            {
                Email = DiverFactory.JohnDoeEmail,
                PhoneNumber = "424242",
            };
            
            // Act
            var diver = new Diver(user, DiverFactory.JohnDoeFirstName, DiverFactory.JohnDoeLastName);

            // Assert
            diver.Should().NotBeNull();
            diver.Firstname.Should().Be(DiverFactory.JohnDoeFirstName);
            diver.Lastname.Should().Be(DiverFactory.JohnDoeLastName);
            diver.MobilePhone.Should().Be("424242");
            diver.Fullname.Should().Be($"{DiverFactory.JohnDoeFirstName} {DiverFactory.JohnDoeLastName}");
        }

        [Fact]
        public void Ctor_NullIdentityUser_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Diver(null, "a", "b");
            
            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("identityUser");
        }

        [Fact]
        public void Ctor_NullFirstName_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Diver(null, "a", "b");
            
            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("identityUser");
        }
        
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

        [Fact]
        public void ChangeAvatar()
        {
            // Arrange
            var diver = DiverFactory.CreateJaneDoe();
            
            // Act
            diver.ChangeAvatar("the_new_avatar_id");
            
            // Assert
            diver.AvatarId.Should().Be("the_new_avatar_id");
            diver.UncommittedDomainEvents.Should().ContainSingle(e => e.GetType() == typeof(AvatarChangedEvent));
        }
    }
}