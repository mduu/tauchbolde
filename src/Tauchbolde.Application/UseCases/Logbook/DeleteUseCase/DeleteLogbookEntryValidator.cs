using FluentValidation;

namespace Tauchbolde.Application.UseCases.Logbook.DeleteUseCase
{
    public class DeleteLogbookEntryValidator : AbstractValidator<DeleteLogbookEntry>
    {
        public DeleteLogbookEntryValidator()
        {
            RuleFor(e => e.LogbookEntryId).NotEmpty();
        }
    }
}