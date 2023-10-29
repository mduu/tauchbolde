using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Events.Details
{
    public class EventCommentViewModel
    {
        public EventCommentViewModel(
            Guid commentId,
            Guid commentAuthorId,
            [NotNull] string commentAuthorEmail,
            [NotNull] string commentAuthorFullname,
            string commentAuthorAvatarId,
            [NotNull] string createdTime,
            string modifiedTime,
            [NotNull] string text,
            bool editAllowed,
            bool deleteAllowed)
        {
            CommentId = commentId;
            CommentAuthorId = commentAuthorId;
            CommentAuthorEmail = commentAuthorEmail ?? throw new ArgumentNullException(nameof(commentAuthorEmail));
            CommentAuthorFullname = commentAuthorFullname ?? throw new ArgumentNullException(nameof(commentAuthorFullname));
            CommentAuthorAvatarId = commentAuthorAvatarId;
            CreatedTime = createdTime ?? throw new ArgumentNullException(nameof(createdTime));
            ModifiedTime = modifiedTime;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            EditAllowed = editAllowed;
            DeleteAllowed = deleteAllowed;
        }

        public Guid CommentId { get; }
        public Guid CommentAuthorId { get; }
        public string CommentAuthorEmail { get; }
        public string CommentAuthorFullname { get; }
        public string CommentAuthorAvatarId { get; }
        public string CreatedTime { get; }
        public string ModifiedTime { get; }
        public string Text { get; }
        public bool EditAllowed { get; }
        public bool DeleteAllowed { get; }
    }
}