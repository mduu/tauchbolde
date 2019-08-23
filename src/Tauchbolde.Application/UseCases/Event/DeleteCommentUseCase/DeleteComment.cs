using System;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.DeleteCommentUseCase
{
    public class DeleteComment : IRequest<UseCaseResult>
    {
        public DeleteComment(Guid commentId, Guid currentUserId)
        {
            CommentId = commentId;
            CurrentUserId = currentUserId;
        }

        public Guid CommentId { get; }
        public Guid CurrentUserId { get; }
    }
}