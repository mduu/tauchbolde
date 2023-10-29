using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.DeleteCommentUseCase
{
    public class DeleteComment : IRequest<UseCaseResult>
    {
        public DeleteComment(Guid commentId)
        {
            CommentId = commentId;
        }

        public Guid CommentId { get; }
    }
}