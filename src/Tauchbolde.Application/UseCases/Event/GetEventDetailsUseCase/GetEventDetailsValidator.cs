using FluentValidation;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase
{
    [UsedImplicitly]
    public class GetEventDetailsValidator : AbstractValidator<GetEventDetails>
    {
        public GetEventDetailsValidator()
        {
            RuleFor(e => e.EventId).NotEmpty();
            RuleFor(e => e.Presenter).NotNull();
        }
    }
}