using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.Application.UseCases.Event.EditCommentUseCase
{
    [UsedImplicitly]
    public class EditCommentInteractor : IRequestHandler<EditComment, UseCaseResult>
    {
        private readonly ICommentRepository commentRepository;

        public EditCommentInteractor([NotNull] ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        }

        public async Task<UseCaseResult> Handle([NotNull] EditComment request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var comment = await commentRepository.FindByIdAsync(request.CommentId);
            if (comment == null)
            {
                return UseCaseResult.NotFound(new[]
                {
                    new ValidationFailure(nameof(request.CommentId), $"Comment [{request.CommentId}] not found!")
                });
            }

            comment.Edit(request.Text);

            try
            {
                await commentRepository.UpdateAsync(comment);
            }
            catch (Exception ex)
            {
                return UseCaseResult.Fail(new[]
                {
                    new ValidationFailure(null, ex.UnwindMessage()),
                });
            }

            return UseCaseResult.Success();
        }
    }
}