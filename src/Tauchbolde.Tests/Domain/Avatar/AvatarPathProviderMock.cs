using System;
using Tauchbolde.Common.Domain.Avatar;

namespace Tauchbolde.Tests.Domain.Avatar
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
