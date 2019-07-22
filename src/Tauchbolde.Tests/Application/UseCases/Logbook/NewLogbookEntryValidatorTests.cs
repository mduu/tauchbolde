using System;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Logbook.NewUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class NewLogbookEntryValidatorTests
    {
        private readonly NewLogbookEntryValidator validator = new NewLogbookEntryValidator();

        [Fact]
        public void Validate_Valid()
        {
            var useCase = CreateNewLogbookEntry("The Title", "The Text");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_EmptyTitleIsInvalid()
        {
            var useCase = CreateNewLogbookEntry("", "The Text");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(NewLogbookEntry.Title));
        }

        [Fact]
        public void Validate_EmptyTextIsInvalid()
        {
            var useCase = CreateNewLogbookEntry("The Title", "");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(NewLogbookEntry.Text));
        }
  
        private static NewLogbookEntry CreateNewLogbookEntry(string title, string text) =>
            new NewLogbookEntry(
                new Guid("31D4B2B7-BE14-4334-A342-110FC30B62CD"),
                title,
                text,
                null,
                false,
                null,
                null,
                null,
                null);
    }
}