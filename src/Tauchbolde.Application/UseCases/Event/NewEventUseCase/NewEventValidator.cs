using FluentValidation;

namespace Tauchbolde.Application.UseCases.Event.NewEventUseCase
{
    internal class NewEventValidator : AbstractValidator<NewEvent>
    {
        public NewEventValidator()
        {
            RuleFor(e => e.CurrentUserName).NotEmpty();
            RuleFor(e => e.EndTime).GreaterThan(e => e.StartTime).When(e => e.EndTime.HasValue);
            RuleFor(e => e.Title).NotEmpty();
            RuleFor(e => e.Location).NotEmpty();
            RuleFor(e => e.MeetingPoint).NotNull();
            RuleFor(e => e.Description).NotNull();
        }
    }
}