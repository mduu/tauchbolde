using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.EditCommentUseCase
{
    public class EditComment : IRequest<UseCaseResult>
    {
        public EditComment(Guid commentId, [NotNull] string text)
        {
            CommentId = commentId;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public Guid CommentId { get; }
        [NotNull] public string Text { get; }
    }
}