using Tauchbolde.Application.Services.Avatars;

namespace Tauchbolde.Tests.Application.Services.Avatars
{
    public class AvatarPathProviderMock : AvatarPathProviderBase
    {
        private readonly string rootPath;

        public AvatarPathProviderMock(string rootPath)
        {
            this.rootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
        }

        protected override string GetRootPath() => rootPath;
    }
}
