using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Administration.GetMassMailUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Administration
{
    public class GetMassMailDetailsInteractorTests
    {
        private readonly GetMassMailDetailsInteractor interactor;
        private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();

        public GetMassMailDetailsInteractorTests()
        {
            A.CallTo(() => diverRepository.GetAllTauchboldeUsersAsync(A<bool>._))
                .ReturnsLazily(() => Task.FromResult<ICollection<Diver>>(
                    DiverFactory.GetTauchbolde().ToList()));
            
            interactor = new GetMassMailDetailsInteractor(diverRepository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var outputPort = A.Fake<IGetMassMailDetailsOutputPort>();
            var request = new GetMassMailDetails(outputPort);
        
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(
                A<GetMassMailDetailsOutput>.That.Matches(m =>
                    m.MailRecipients.Count() == 2)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Handle_NullRequest_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);
            
            // Assert
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("request");
        }
    }
}