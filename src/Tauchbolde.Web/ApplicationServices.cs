﻿using System;
using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Web.Services;
using Tauchbolde.Web.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Tauchbolde.Common;
using Tauchbolde.Common.Domain;
using Tauchbolde.Common.Domain.Avatar;
using Tauchbolde.Web.Core.TextFormatting;

namespace Tauchbolde.Web
{
    [UsedImplicitly]
    public class ApplicationServices
    {
        public static void Register(
            [NotNull] IServiceCollection services,
            [NotNull] IConfiguration configuration,
            [NotNull] IHostingEnvironment hostingEnvironment)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (hostingEnvironment == null) throw new ArgumentNullException(nameof(hostingEnvironment));

            var photoStoreRoot = configuration["PhotoStoreRoot"];
            if (string.IsNullOrWhiteSpace(photoStoreRoot))
            {
                photoStoreRoot = Path.Combine(hostingEnvironment.WebRootPath, "photos");
            }
            
            CommonServices.RegisterServices(services, photoStoreRoot);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlGenerator, MvcUrlGenerator>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IAvatarPathProvider, AvatarPathProvider>();
            services.AddTransient<ITextFormattingHelper, TextFormattingHelper>();
        }

        public static void RegisterDevelopment(IServiceCollection services)
        {
            if (services == null) { throw new System.ArgumentNullException(nameof(services)); }

            CommonServices.RegisterDevelopment(services);
        }

        public static void RegisterProduction(IServiceCollection services)
        {
            if (services == null) { throw new System.ArgumentNullException(nameof(services)); }

            CommonServices.RegisterProduction(services);
        }
    }
}
