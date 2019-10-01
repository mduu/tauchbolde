using FluentValidation;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Logbook.NewUseCase
{
    [UsedImplicitly]
    internal class NewLogbookEntryValidator : AbstractValidator<NewLogbookEntry>
    {
        public NewLogbookEntryValidator()
        {
            RuleFor(e => e.Title).NotEmpty().WithMessage("Titel darf nicht leer sein!");
            RuleFor(e => e.Teaser).NotEmpty().WithMessage("Teaser darf nicht leer sein!");
            RuleFor(e => e.Text).NotEmpty().WithMessage("Text darf nicht leer sein!");
        }
    }
}