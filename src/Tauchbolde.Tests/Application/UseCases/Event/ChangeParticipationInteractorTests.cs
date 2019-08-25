using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Event.ChangeParticipationUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class ChangeParticipationInteractorTests
    {
        private const string ValidUserName = "jon.doe";
        private readonly Guid validEventId = new Guid("03A09D19-4DCB-41D7-B997-9C84E9B386FE");
        private readonly Guid validParticipantId = new Guid("D9A6A368-1129-404C-9A2D-8285B7A44D6D");
        private readonly Guid validDiverId = new Guid("EE4200E6-CAB4-488B-B84E-135782A35606");
        private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();
        private readonly IParticipantRepository participantRepository = A.Fake<IParticipantRepository>();
        private readonly ChangeParticipationInteractor interactor;

        public ChangeParticipationInteractorTests()
        {
            interactor = new ChangeParticipationInteractor(diverRepository, participantRepository);
        }

        [Fact]
        public async Task Handle_NewParticipant_Success()
        {
            // Arrange
            var request = CreateValidChangeParticipationRequest();
            A.CallTo(() => participantRepository.GetParticipationForEventAndUserAsync(A<Diver>._, A<Guid>._))
                .ReturnsLazily(call => Task.FromResult<Participant>(null));

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => participantRepository.InsertAsync(A<Participant>._))
                .MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_UpdateExistingParticipant_Success()
        {
            // Arrange
            var request = CreateValidChangeParticipationRequest();
            A.CallTo(() => participantRepository.GetParticipationForEventAndUserAsync(A<Diver>._, A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(CreateValidParticipant()));

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => participantRepository.UpdateAsync(A<Participant>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_NullRequest_MustFail()
        {
            // Act
            Func<Task> act = async () => await interactor.Handle(null, CancellationToken.None);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        private Participant CreateValidParticipant()
        {
            return new Participant
            {
                Id = validParticipantId,
                ParticipatingDiverId = validDiverId,
                EventId = validEventId,
                Status = ParticipantStatus.None,
                CountPeople = 1,
            };
        }

        private ChangeParticipation CreateValidChangeParticipationRequest() =>
            new ChangeParticipation(ValidUserName,
                validEventId,
                ParticipantStatus.Accepted,
                1,
                null,
                null);
    }
}