using System.Linq;
using Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.Event.Details
{
    public class MvcEventDetailPresenter : IEventDetailsOutputPort
    {
        private EventViewModel viewModel;

        public void Output(GetEventDetailsOutput interactorOutput)
        {
            viewModel = new EventViewModel(
                interactorOutput.AllowEdit,
                interactorOutput.EventId,
                interactorOutput.Name,
                interactorOutput.OrganisatorId,
                interactorOutput.OrganisatorFullName,
                interactorOutput.Location,
                interactorOutput.MeetingPoint,
                interactorOutput.Description,
                interactorOutput.StartTime.FormatTimeRange(interactorOutput.EndTime),
                interactorOutput.Comments
                    .OrderBy(c => c.CreatedTime)
                    .Select(c => new EventCommentViewModel(
                        c.CommentId,
                        c.AuthorId,
                        c.AuthorEmail,
                        c.AuthorName,
                        c.AuthorAvatarId,
                        c.CreatedTime.ToStringSwissDateTime(),
                        c.ModifiedTime?.ToStringSwissDateTime(),
                        c.Text,
                        c.AllowEdit,
                        c.AllowDelete)),
                new EventParticipationViewModel(
                    interactorOutput.EventId,
                    BuddyTeamNames.Names,
                    interactorOutput.Participants.Select(p => new EventParticipantViewModel(
                        p.Name,
                        p.Email,
                        p.AvatarId,
                        p.BuddyTeamName,
                        p.Status.ToString(),
                        p.Status,
                        p.Note,
                        p.CountPeople
                    )),
                    interactorOutput.CurrentUserStatus,
                    interactorOutput.CurrentUserNote,
                    interactorOutput.CurrentUserBuddyTeamName,
                    interactorOutput.CurrentUserCountPeople));
        }

        public EventViewModel GetViewModel() => viewModel;
    }
}