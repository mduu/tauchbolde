using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Event.EditCommentUseCase;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class EditCommentInteractorTests
    {
        private readonly Guid validCommentId = new Guid("A07F490E-148D-4DCF-B1BC-75B790BB16E3");
        private readonly ICommentRepository commentRepository = A.Fake<ICommentRepository>();
        private readonly EditCommentInteractor interactor;

        public EditCommentInteractorTests()
        {
            A.CallTo(() => commentRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validCommentId
                        ? new Comment
                        {
                            Id = validCommentId,
                            CreateDate = new DateTime(2019, 8, 20, 8, 0, 0),
                            AuthorId = new Guid("0E0D555C-B742-4BE2-B4F7-C9724F02EFC4"),
                            EventId = new Guid("6EB3538F-9C98-43C2-9EE2-C0C9B2C52D2F"),
                            Text = "A comment"
                        }
                        : null
                ));

            interactor = new EditCommentInteractor(commentRepository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var newText = "a new comment";
            var request = new EditComment(validCommentId, newText);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => commentRepository.UpdateAsync(
                    A<Comment>.That.Matches(c =>
                        c.Id.ToString("B") == validCommentId.ToString("B") &&
                        c.Text == newText)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_CommentNotFoundMustFail()
        {
            // Arrange
            var newAuthorId = new Guid("B6691056-FEFF-44EE-8FE1-A23565CD25E6");
            var newText = "a new comment";
            var request = new EditComment(new Guid("212B73C2-452E-4E1E-B3D7-C3DF6154C85E"), newText);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            result.IsSuccessful.Should().BeFalse();
            A.CallTo(() => commentRepository.UpdateAsync(A<Comment>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_UpdateFail()
        {
            // Arrange
            var newAuthorId = new Guid("B6691056-FEFF-44EE-8FE1-A23565CD25E6");
            var newText = "a new comment";
            var request = new EditComment(validCommentId, newText);
            A.CallTo(() => commentRepository.UpdateAsync(A<Comment>._))
                .Invokes(() => throw new InvalidOperationException());

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Fact]
        public void Handle_NullRequestMustFail()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);
            
            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}