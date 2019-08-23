using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.DeleteCommentUseCase
{
    [UsedImplicitly]
    public class DeleteCommentInteractor : IRequestHandler<DeleteComment, UseCaseResult>
    {
        [NotNull] private readonly ICommentRepository repository;

        public DeleteCommentInteractor([NotNull] ICommentRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<UseCaseResult> Handle([NotNull] DeleteComment request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var comment = await repository.FindByIdAsync(request.CommentId);
            if (comment == null)
            {
                return UseCaseResult.NotFound(new[]
                {
                    new ValidationFailure(nameof(request.CommentId), $"Comment [{request.CommentId}] not found!")
                });
            }
            
            comment.Delete();

            await repository.DeleteAsync(comment);
            
            return UseCaseResult.Success();
        }
    }
}