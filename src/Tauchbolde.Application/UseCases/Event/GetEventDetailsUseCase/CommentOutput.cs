using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase
{
    public class CommentOutput
    {
        public CommentOutput(
            Guid commentId,
            Guid authorId,
            [NotNull] string authorName,
            [NotNull] string authorEmail,
            [CanBeNull] string authorAvatarId,
            DateTime createdTime,
            DateTime? modifiedTime,
            [NotNull] string text,
            bool allowEdit,
            bool allowDelete)
        {
            CommentId = commentId;
            AuthorId = authorId;
            AuthorName = authorName ?? throw new ArgumentNullException(nameof(authorName));
            CreatedTime = createdTime;
            ModifiedTime = modifiedTime;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            AllowEdit = allowEdit;
            AllowDelete = allowDelete;
            AuthorEmail = authorEmail;
            AuthorAvatarId = authorAvatarId;
        }

        public Guid CommentId { get; }
        public Guid AuthorId { get; }
        [NotNull] public string AuthorName { get; }
        [NotNull] public string AuthorEmail { get; }
        [CanBeNull] public string AuthorAvatarId { get; }
        public DateTime CreatedTime { get; }
        public DateTime? ModifiedTime { get; }
        [NotNull] public string Text { get; }
        public bool AllowEdit { get; }
        public bool AllowDelete { get; }
    }
}