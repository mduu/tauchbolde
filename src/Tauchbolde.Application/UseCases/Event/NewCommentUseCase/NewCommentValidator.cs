using FluentValidation;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.NewCommentUseCase
{
    [UsedImplicitly]
    public class NewCommentValidator : AbstractValidator<NewComment>
    {
        public NewCommentValidator()
        {
            RuleFor(u => u.EventId).NotEmpty();
            RuleFor(u => u.Text).NotEmpty();
        }
    }
}