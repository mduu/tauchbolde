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
            var useCase = CreateNewLogbookEntry("The Title", "The Teaser", "The Text");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_EmptyTitleIsInvalid()
        {
            var useCase = CreateNewLogbookEntry("", "The Teaser", "The Text");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(NewLogbookEntry.Title));
        }

        [Fact]
        public void Validate_EmptyTeaserIsInvalid()
        {
            var useCase = CreateNewLogbookEntry("The Title", "", "The Text");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(NewLogbookEntry.Teaser));
        }

        [Fact]
        public void Validate_EmptyTextIsInvalid()
        {
            var useCase = CreateNewLogbookEntry("The Title", "The Teaser", "");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(NewLogbookEntry.Text));
        }
  
        private static NewLogbookEntry CreateNewLogbookEntry(string title, string teaser, string text) =>
            new NewLogbookEntry(
                title,
                teaser,
                text,
                false,
                null,
                null,
                null, null);
    }
}