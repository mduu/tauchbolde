using System;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Logbook.EditUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class EditLogbookEntryValidatorTests
    {
        private readonly Guid validLogbookEntryId = new Guid("234E56E5-A23D-4FA3-A295-3DF99F616073");
        private readonly EditLogbookEntryValidator validator = new EditLogbookEntryValidator();
        
        [Fact]
        public void Validate_Valid()
        {
            var useCase = CreateEditLogbookEntry("The Title", "The Teaser", "The Text");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_EmptyLogbookEntryId()
        {
            var useCase = CreateEditLogbookEntry("", "The Teaser", "The Text", Guid.Empty);

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(EditLogbookEntry.LogbookEntryId));
        }

        [Fact]
        public void Validate_EmptyTitleIsEmpty()
        {
            var useCase = CreateEditLogbookEntry("", "The Teaser", "The Text");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(EditLogbookEntry.Title));
        }

        [Fact]
        public void Validate_EmptyTeaserIsEmpty()
        {
            var useCase = CreateEditLogbookEntry("The Title", "", "The Text");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(EditLogbookEntry.Teaser));
        }

        [Fact]
        public void Validate_EmptyTextIsEmpty()
        {
            var useCase = CreateEditLogbookEntry("The Title", "The Teaser", "");

            var result = validator.Validate(useCase);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(EditLogbookEntry.Text));
        }
  
        private EditLogbookEntry CreateEditLogbookEntry(string title, string teaser, string text,
            Guid? logbookEntryId = null) =>
            new EditLogbookEntry(
                logbookEntryId ?? validLogbookEntryId,
                new Guid("31D4B2B7-BE14-4334-A342-110FC30B62CD"),
                title,
                teaser,
                text,
                false,
                null,
                null,
                null, null);

    }
}