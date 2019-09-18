using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.NewCommentUseCase
{
    public class NewComment : IRequest<UseCaseResult>
    {
        public NewComment(Guid eventId, [NotNull] string text)
        {
            EventId = eventId;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public Guid EventId { get; }
        [NotNull] public string Text { get; }
    }
}