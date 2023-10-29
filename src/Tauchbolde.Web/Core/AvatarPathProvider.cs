using Tauchbolde.Application.Services.Avatars;

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
        private readonly IWebHostEnvironment hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Web.Core.AvatarPathProvider"/> class.
        /// </summary>
        /// <param name="environment">Environment.</param>
        public AvatarPathProvider(IWebHostEnvironment environment)
        {
            hostingEnvironment = environment;
        }

        protected override string GetRootPath()
            => Path.Combine(
                hostingEnvironment.ContentRootPath,
                "wwwroot");
    }
}
