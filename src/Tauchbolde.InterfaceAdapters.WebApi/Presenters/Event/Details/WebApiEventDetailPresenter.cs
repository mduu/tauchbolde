using System;
using System.Linq;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase;

namespace Tauchbolde.InterfaceAdapters.WebApi.Presenters.Event.Details
{
    public class WebApiEventDetailPresenter : IEventDetailsOutputPort
    {
        private object jsonObject;

        public void Output([NotNull] GetEventDetailsOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            jsonObject = new
            {
                title = output.Name,
                description = output.Description,
                location = output.Location,
                meetingPoint = output.MeetingPoint,
                organisator = output.OrganisatorFullName,
                allowEdit = output.AllowEdit,
                canceled = output.Canceled,
                deleted = output.Deleted,
                startTime = output.StartTime.ToString("O"),
                endTime = output.EndTime?.ToString("O"),
                participants = output.Participants.Select(p => new
                {
                    name = p.Name,
                    email = p.Email,
                    note = p.Note,
                    status = p.Status.ToString(),
                    avatarId = p.AvatarId,
                    buddyTeamName = p.BuddyTeamName,
                    countPeople = p.CountPeople,
                }),
                comments = output.Comments.Select(c => new
                {
                    commentId = c.CommentId,
                    text = c.Text,
                    authorId = c.AuthorId,
                    authorEmail = c.AuthorEmail,
                    authorName = c.AuthorName,
                    authorAvatarId = c.AuthorAvatarId,
                    createdTime = c.CreatedTime.ToString("O"),
                    modifiedTime = c.ModifiedTime?.ToString("O") ?? null,
                    allowEdit = c.AllowEdit,
                    allowDelete = c.AllowDelete,
                })
            };
        }

        public object GetJsonObject() => jsonObject;
    }
}