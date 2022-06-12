using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Event.NewCommentUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class NewCommentInteractorTests
    {
        private readonly Guid validEventId = new("6BE85F8D-3898-488C-92A9-D979648B742B");
        private readonly Guid validAuthorId = new("E072BCB8-36E6-4E36-A241-31E4DDB14435");
        private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
        private readonly ICommentRepository commentRepository = A.Fake<ICommentRepository>();
        private readonly NewCommentInteractor interactor;
        private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();

        public NewCommentInteractorTests()
        {
            A.CallTo(() => eventRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validEventId
                        ? new Tauchbolde.Domain.Entities.Event { Id = validEventId }
                        : null
                    ));

            A.CallTo(() => currentUser.GetCurrentDiverAsync())
                .ReturnsLazily(() => Task.FromResult(
                    new Diver
                    {
                        Id = validAuthorId,
                        Fullname = "John Doe"
                    }));

            interactor = new NewCommentInteractor(eventRepository, commentRepository, currentUser);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var request = new NewComment(validEventId, "The answer is 42!");
            
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            useCaseResult.Should().NotBeNull();
            useCaseResult.IsSuccessful.Should().BeTrue();
            useCaseResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_InvalidEventId()
        {
            var request = new NewComment(
                new Guid("15D08C24-FD8F-43E8-989E-69B98E8257B0"), 
                "The answer is 42!");
            
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            useCaseResult.Should().NotBeNull();
            useCaseResult.IsSuccessful.Should().BeFalse();
            useCaseResult.ResultCategory.Should().Be(ResultCategory.NotFound);
            useCaseResult.Errors.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_ErrorWhileInserting()
        {
            var request = new NewComment(validEventId, "The answer is 42!");
            A.CallTo(() => commentRepository.InsertAsync(A<Comment>._))
                .Invokes(() => throw new InvalidOperationException());
            
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            useCaseResult.Should().NotBeNull();
            useCaseResult.IsSuccessful.Should().BeFalse();
            useCaseResult.ResultCategory.Should().Be(ResultCategory.GeneralFailure);
            useCaseResult.Errors.Should().HaveCount(1);
        }
    }
}