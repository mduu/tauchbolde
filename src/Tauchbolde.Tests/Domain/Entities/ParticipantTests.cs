using FluentAssertions;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Xunit;

namespace Tauchbolde.Tests.Domain.Entities
{
    public class ParticipantTests
    {
        private readonly Guid validParticipantId = new Guid("40575568-FC15-4966-81A9-046ECA856674");
        private readonly Guid validEventId = new Guid("6DCAF731-EF4D-4C92-B7AF-FBDE48325B29");
        private readonly Guid validDiverId = new Guid("43F1E95C-5211-4031-A0FA-EFD95ADE425B");

        [Theory]
        [InlineData(ParticipantStatus.Accepted)]
        [InlineData(ParticipantStatus.Declined)]
        [InlineData(ParticipantStatus.Tentative)]
        [InlineData(ParticipantStatus.None)]
        public void Ctor_NewParticipationSuccess(ParticipantStatus status)
        {
            // Act
            var participant = new Participant(validEventId, validDiverId, status, "team1", 1, null);

            // Assert
            participant.EventId.Should().Be(validEventId);
            participant.ParticipatingDiverId.Should().Be(validDiverId);
            participant.Status.Should().Be(status);
            participant.CountPeople.Should().Be(1);
            participant.BuddyTeamName.Should().Be("team1");
            participant.Note.Should().BeNull();
            participant.UncommittedDomainEvents.Should().ContainSingle(c => c.GetType() == typeof(ParticipationChangedEvent));
        }
        
        [Fact]
        public void Ctor_NewParticipation_WithNote_Success()
        {
            // Act
            var participant = new Participant(validEventId, validDiverId, ParticipantStatus.Accepted, "team1", 1, "test");

            // Assert
            participant.EventId.Should().Be(validEventId);
            participant.ParticipatingDiverId.Should().Be(validDiverId);
            participant.Status.Should().Be(ParticipantStatus.Accepted);
            participant.CountPeople.Should().Be(1);
            participant.BuddyTeamName.Should().Be("team1");
            participant.Note.Should().Be("test");
        }
        
        [Fact]
        public void Ctor_NewParticipation_WithtLessThenOnePpl_Success()
        {
            // Act
            var participant = new Participant(validEventId, validDiverId, ParticipantStatus.Accepted, "team1", 0, null);

            // Assert
            participant.CountPeople.Should().Be(1);
        }
        
        [Fact]
        public void Ctor_NewParticipation_WithMoreThenOnePpl_Success()
        {
            // Act
            var participant = new Participant(validEventId, validDiverId, ParticipantStatus.Accepted, "team1", 2, null);

            // Assert
            participant.CountPeople.Should().Be(2);
        }

        [Theory]
        [InlineData(ParticipantStatus.Accepted, "team2", 0, null, 1)]
        [InlineData(ParticipantStatus.Declined, null, 1, "I'm not here", 1)]
        [InlineData(ParticipantStatus.Tentative, "team2", 2, "I'm propably here", 2)]
        [InlineData(ParticipantStatus.None, null, -1, null, 1)]
        public void Edit_Succuss(ParticipantStatus status, string buddyTeam, int countPpl, string note, int expectedCountPpl)
        {
            // Arrange
            var participant = new Participant
            {
                Id = validParticipantId,
                EventId = validEventId,
                ParticipatingDiverId = validDiverId,
                BuddyTeamName = "team1",
                CountPeople = 1,
                Status = ParticipantStatus.None,
                Note = null,
            };
            
            // Act
            participant.Edit(status, buddyTeam, countPpl, note);

            // Assert
            participant.Status.Should().Be(status);
            participant.BuddyTeamName.Should().Be(buddyTeam);
            participant.CountPeople.Should().Be(expectedCountPpl);
            participant.Note.Should().Be(note);
            
            participant.Id.Should().Be(validParticipantId);
            participant.EventId.Should().Be(validEventId);
            participant.ParticipatingDiverId.Should().Be(validDiverId);

            participant.UncommittedDomainEvents.Should().ContainSingle(e => e.GetType() == typeof(ParticipationChangedEvent));
        }
    }
}