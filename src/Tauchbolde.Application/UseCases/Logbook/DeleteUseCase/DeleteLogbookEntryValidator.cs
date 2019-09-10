using FluentValidation;

namespace Tauchbolde.Application.UseCases.Logbook.DeleteUseCase
{
    internal class DeleteLogbookEntryValidator : AbstractValidator<DeleteLogbookEntry>
    {
        public DeleteLogbookEntryValidator()
        {
            RuleFor(e => e.LogbookEntryId).NotEmpty();
        }
    }
}