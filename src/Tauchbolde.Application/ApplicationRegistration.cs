using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Application.OldDomainServices;
using Tauchbolde.Application.OldDomainServices.Avatar;
using Tauchbolde.Application.OldDomainServices.Events;
using Tauchbolde.Application.OldDomainServices.Notifications;
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.Services;
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
            
            services.AddMediatR(typeof(PublishLogbookEntryHandler));

            services.AddScoped<IClock, Clock>();
            services.AddSingleton<IMimeMapping, MimeMapping>();
            
            services.AddScoped<INotificationPublisher, NotificationPublisher>();
            services.AddTransient<INotificationSender, NotificationSender>();
            services.AddTransient<INotificationTypeInfos, NotificationTypeInfos>();
            services.AddTransient<IRecipientsBuilder, RecipientsBuilder>();

            services.AddTransient<IPhotoService, PhotoService>();

            RegisterOldDomainServices(services);
        }

        public static void RegisterDevelopment(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }

            services.AddTransient<INotificationSubmitter, ConsoleNotificationSubmitter>();
        }
        
        private static void RegisterOldDomainServices(IServiceCollection services)
        {
            // TODO The goal is to remove all these old "domain services"
            
            services.AddTransient<IParticipationService, ParticipationService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IDiverService, DiversService>();
            services.AddTransient<IMassMailService, MassMailService>();
            
            services.AddSingleton<IAvatarIdGenerator, AvatarIdGenerator>();
            services.AddSingleton<IAvatarStore, AvatarStore>();
        }
    }
}