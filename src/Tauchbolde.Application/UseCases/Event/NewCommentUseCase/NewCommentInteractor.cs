using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.Application.UseCases.Event.NewCommentUseCase
{
    [UsedImplicitly]
    public class NewCommentInteractor : IRequestHandler<NewComment, UseCaseResult>
    {
        private readonly IEventRepository eventRepository;
        private readonly ICommentRepository commentRepository;

        public NewCommentInteractor([NotNull] IEventRepository eventRepository, [NotNull] ICommentRepository commentRepository)
        {
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        }

        public async Task<UseCaseResult> Handle([NotNull] NewComment request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var evt = await eventRepository.FindByIdAsync(request.EventId);
            if (evt == null)
            {
                return UseCaseResult.NotFound(new []
                {
                    new ValidationFailure(nameof(request.EventId), $"Event [{request.EventId}] not found!")
                });
            }
            
            var newComment = evt.AddNewComment(request.AuthorId, request.Text);

            try
            {
                await commentRepository.InsertAsync(newComment);
            }
            catch (Exception ex)
            {
                return UseCaseResult.Fail(new []
                {
                    new ValidationFailure(null, ex.UnwindMessage()),
                });
            }

            return UseCaseResult.Success();
        }
    }
}