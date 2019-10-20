using FluentAssertions;
using Tauchbolde.Application.UseCases.Administration.AddMemberUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Administration
{
    public class AddMemberValidatorTests
    {
        private readonly AddMemberValidator validator = new AddMemberValidator();

        [Fact]
        public void Validate_Success()
        {
            // Act
            var result = validator.Validate(new AddMember("john.doe", "John", "Doe"));
            
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "john", "doe", nameof(AddMember.UserName))]
        [InlineData("john.doe", "", "doe", nameof(AddMember.FirstName))]
        [InlineData("john.doe", "john", "", nameof(AddMember.LastName))]
        public void Validate_UserName_NotEmpty(string username, string firstname, string lastname, string expectedPropToFail)
        {
            // Act
            var result = validator.Validate(new AddMember(username, firstname, lastname));
            
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == expectedPropToFail);
        }
    }
}