using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Event.DeleteCommentUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class DeleteCommentInteractorTests
    {
        private readonly Guid validCommentId = new("F5F4A476-B7D1-49DC-AA68-73FBB93C882B");
        private readonly Guid validUserId = new("F2E454B0-F8AA-476E-898E-865AE6CD5ED5");
        private readonly ICommentRepository repository = A.Fake<ICommentRepository>();
        private readonly DeleteCommentInteractor interactor;

        public DeleteCommentInteractorTests()
        {
            A.CallTo(() => repository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid)call.Arguments[0] == validCommentId
                        ? new Comment { Id = validCommentId, AuthorId = validUserId, Text = "A comment"} 
                        : null
                ));

            interactor = new DeleteCommentInteractor(repository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new DeleteComment(validCommentId);
            
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => repository.DeleteAsync(A<Comment>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_NotFound()
        {
            // Arrange
            var request = new DeleteComment(new Guid("3BE6DCBE-D46D-472D-890E-FE99700830EB"));
            
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => repository.DeleteAsync(A<Comment>._))
                .MustNotHaveHappened();
        }
    }
}