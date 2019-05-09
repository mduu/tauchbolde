using System.IO;
using Microsoft.AspNetCore.Hosting;
using Tauchbolde.Common.Domain.Avatar;

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
        private readonly IHostingEnvironment hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Web.Core.AvatarPathProvider"/> class.
        /// </summary>
        /// <param name="environment">Environment.</param>
        public AvatarPathProvider(IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }

        protected override string GetRootPath()
            => Path.Combine(
                hostingEnvironment.ContentRootPath,
                "wwwroot");
    }
}
