using FluentValidation;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Logbook.EditUseCase
{
    [UsedImplicitly]
    public class EditLogbookEntryValidator : AbstractValidator<EditLogbookEntry>
    {
        public EditLogbookEntryValidator()
        {
            RuleFor(e => e.LogbookEntryId).NotEmpty().WithMessage("Ungültige Logbuch-Eintrag ID!");
            RuleFor(e => e.EditorDiverId).NotEmpty().WithMessage("Ungültige Autor-ID!");
            RuleFor(e => e.Title).NotEmpty().WithMessage("Titel darf nicht leer sein!");
            RuleFor(e => e.Teaser).NotEmpty().WithMessage("Teaser darf nicht leer sein!");
            RuleFor(e => e.Text).NotEmpty().WithMessage("Text darf nicht leer sein!");
        }        
    }
}