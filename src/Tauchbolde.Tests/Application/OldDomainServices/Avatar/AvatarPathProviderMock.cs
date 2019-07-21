using System;
using Tauchbolde.Application.OldDomainServices.Avatar;

namespace Tauchbolde.Tests.Application.OldDomainServices.Avatar
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
