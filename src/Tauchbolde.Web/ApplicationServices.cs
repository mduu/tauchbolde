using System;
using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Tauchbolde.Application;
using Tauchbolde.Application.Services.Avatars;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Driver.DataAccessSql;
using Tauchbolde.Driver.ApplicationInsights;
using Tauchbolde.Driver.ImageSharp;
using Tauchbolde.Driver.PhotoStorage;
using Tauchbolde.Driver.SmtpEmail;
using Tauchbolde.InterfaceAdapters;
using Tauchbolde.InterfaceAdapters.Logbook.Details;
using Tauchbolde.SharedKernel;
using Tauchbolde.Web.Services;
using Tauchbolde.Web.Core;
using Tauchbolde.Web.Core.TextFormatting;
using Tauchbolde.Web.Core.TokenHandling;
using Tauchbolde.Web.Core.UrlGeneration;

namespace Tauchbolde.Web
{
    [UsedImplicitly]
    public static class ApplicationServices
    {
        public static void Register(
            [NotNull] IServiceCollection services,
            [NotNull] IConfiguration configuration,
            [NotNull] IHostingEnvironment hostingEnvironment)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (hostingEnvironment == null) throw new ArgumentNullException(nameof(hostingEnvironment));

            var photoStoreRoot = GetPhotoStoreRoot(configuration, hostingEnvironment);
            var photoStoreType = GetPhotoStoreType(configuration);

            SharedKernelRegistrations.RegisterServices(services);
            ApplicationRegistration.RegisterServices(services);
            InterfaceAdaptersRegistration.RegisterServices(services);

            PhotoStorageRegistration.RegisterServices(services, photoStoreRoot, photoStoreType);
            ApplicationInsightsRegistration.RegisterServices(services);
            ImageSharpRegistrations.RegisterServices(services);
            SmtpEmailRegistrations.RegisterServices(services);
            DataAccessServices.RegisterServices(services);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IAbsoluteUrlGenerator, MvcAbsoluteUrlGenerator>();
            services.AddSingleton<IRelativeUrlGenerator, MvcRelativeUrlGenerator>();
            services.AddSingleton<ILogbookDetailsUrlGenerator, MvcLogbookDetailsUrlGenerator>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddSingleton<IAvatarPathProvider, AvatarPathProvider>();
            services.AddTransient<ITextFormattingHelper, TextFormattingHelper>();
            services.AddSingleton(new TokenConfiguration(GetTokenSecurityKey(configuration)));
            services.AddTransient<ITokenGenerator, TokenGenerator>();
        }

        public static void RegisterDevelopment(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }

            ApplicationRegistration.RegisterDevelopment(services);
        }

        public static void RegisterProduction(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }

            SmtpEmailRegistrations.RegisterServices(services);
        }

        private static string GetTokenSecurityKey(IConfiguration configuration)
        {
            var tokenSecurityKey = configuration["TokenSecurityKey"];

            return string.IsNullOrWhiteSpace(tokenSecurityKey)
                ? throw new InvalidOperationException("Not 'TokenSecurityKey' configured!")
                : tokenSecurityKey;
        }

        private static string GetPhotoStoreRoot(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            var photoStoreRoot = configuration["PhotoStoreRoot"];
            if (string.IsNullOrWhiteSpace(photoStoreRoot))
            {
                photoStoreRoot = Path.Combine(hostingEnvironment.WebRootPath, "photos");
            }

            return photoStoreRoot;
        }

        private static PhotoStoreType GetPhotoStoreType(IConfiguration configuration)
        {
            var photoStoreType = PhotoStoreType.FileSystem;
            var photoStoreTypeString = configuration["PhotoStoreType"];
            if (!string.IsNullOrWhiteSpace(photoStoreTypeString))
            {
                Enum.TryParse(photoStoreTypeString, out photoStoreType);
            }

            return photoStoreType;
        }
    }
}