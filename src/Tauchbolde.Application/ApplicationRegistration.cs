using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Application.Services;
using Tauchbolde.Application.Services.Avatars;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Application.UseCases.Logbook.PublishUseCase;

[assembly: InternalsVisibleTo("Tauchbolde.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // For FakeItEasy to use "internal" visibility

namespace Tauchbolde.Application
{
    public static class ApplicationRegistration
    {
        public static void RegisterServices([NotNull] IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddMediatR(typeof(PublishLogbookEntryInteractor));

            services.AddScoped<IClock, Clock>();
            services.AddScoped<ICurrentUserInformation, CurrentUserInformation>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddSingleton<IMimeMapping, MimeMapping>();
            
            services.AddSingleton<IAvatarIdGenerator, AvatarIdGenerator>();
            services.AddSingleton<IAvatarStore, AvatarStore>();

            services.AddScoped<INotificationPublisher, NotificationPublisher>();
            services.AddTransient<INotificationSender, NotificationSender>();
            services.AddTransient<INotificationTypeInfos, NotificationTypeInfos>();
            services.AddTransient<IRecipientsBuilder, RecipientsBuilder>();

            services.AddTransient<IPhotoService, PhotoService>();
        }

        public static void RegisterDevelopment(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }

            services.AddTransient<INotificationSubmitter, ConsoleNotificationSubmitter>();
        }
    }
}