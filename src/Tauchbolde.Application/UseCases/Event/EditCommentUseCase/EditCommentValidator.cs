using FluentValidation;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.EditCommentUseCase
{
    [UsedImplicitly]
    public class EditCommentValidator : AbstractValidator<EditComment>
    {
        public EditCommentValidator()
        {
            RuleFor(u => u.CommentId).NotEmpty();
            RuleFor(u => u.AuthorId).NotEmpty();
            RuleFor(u => u.Text).NotEmpty();
        }
    }
}