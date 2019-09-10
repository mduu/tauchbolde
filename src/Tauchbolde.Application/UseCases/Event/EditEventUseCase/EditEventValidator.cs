using FluentValidation;

namespace Tauchbolde.Application.UseCases.Event.EditEventUseCase
{
    public class EditEventValidator : AbstractValidator<EditEvent>
    {
        public EditEventValidator()
        {
            RuleFor(e => e.EventId).NotEmpty();
            RuleFor(e => e.EndTime).GreaterThan(e => e.StartTime).When(e => e.EndTime.HasValue);
            RuleFor(e => e.Title).NotEmpty();
            RuleFor(e => e.Location).NotEmpty();
            RuleFor(e => e.MeetingPoint).NotNull();
            RuleFor(e => e.Description).NotNull();
        }
    }
}