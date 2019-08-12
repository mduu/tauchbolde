using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.NewCommentUseCase
{
    public class NewComment : IRequest<UseCaseResult>
    {
        public NewComment(Guid eventId, Guid authorId, [NotNull] string text)
        {
            EventId = eventId;
            AuthorId = authorId;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public Guid EventId { get; }
        public Guid AuthorId { get; }
        [NotNull] public string Text { get; }
    }
}