using System;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Logbook.DeleteUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class DeleteLogbookEntryValidatorTests
    {
        private readonly DeleteLogbookEntryValidator validator = new DeleteLogbookEntryValidator();

        [Fact]
        public void Validate_Success()
        {
            var request = new DeleteLogbookEntry(new Guid("F9AD80DE-50A2-4229-9A1D-69CFDE231A4E"));

            var validationResult = validator.Validate(request);

            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_EmptyIdIsInvalid()
        {
            var request = new DeleteLogbookEntry(Guid.Empty);

            var validationResult = validator.Validate(request);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should()
                .ContainSingle(e => e.PropertyName == nameof(DeleteLogbookEntry.LogbookEntryId));
        }
    }
}