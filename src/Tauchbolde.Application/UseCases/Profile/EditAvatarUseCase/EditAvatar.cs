using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.EditAvatarUseCase
{
    public class EditAvatar : IRequest<UseCaseResult>
    {
        public EditAvatar(Guid userId, [CanBeNull] AvatarFile avatar)
        {
            UserId = userId;
            Avatar = avatar;
        }

        public Guid UserId { get; }
        public AvatarFile Avatar { get; }

        public class AvatarFile
        {
            public AvatarFile([NotNull] string filename, [NotNull] string contentType, [NotNull] Stream content)
            {
                Filename = filename ?? throw new ArgumentNullException(nameof(filename));
                ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
                Content = content ?? throw new ArgumentNullException(nameof(content));
            }

            public string Filename { get; }
            public string ContentType { get; }
            public Stream Content { get; }
        }
    }
}