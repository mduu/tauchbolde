using System.IO;
using Microsoft.AspNetCore.Hosting;
using Tauchbolde.Common.DomainServices.Avatar;

namespace Tauchbolde.Web.Core
{
    /// <summary>
    /// Implements <see cref="IAvatarPathProvider"/> using <see cref="IHostingEnvironment"/>.
    /// </summary>
    /// <seealso cref="IAvatarPathProvider"/>
    /// <seealso cref="IHostingEnvironment"/>
    /// <seealso cref="IAvatarStore"/>
    public class AvatarPathProvider : AvatarPathProviderBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Web.Core.AvatarPathProvider"/> class.
        /// </summary>
        /// <param name="environment">Environment.</param>
        public AvatarPathProvider(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        protected override string GetRootPath() => _hostingEnvironment.ContentRootPath;
    }
}
