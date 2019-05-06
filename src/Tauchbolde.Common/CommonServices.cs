using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Commom.Misc;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Common.DomainServices.Avatar;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.DomainServices.SMTPSender;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DataAccess;
using Tauchbolde.Common.Infrastructure.Telemetry;
using Tauchbolde.Common.DomainServices.Events;
using Tauchbolde.Common.DomainServices.Logbook;
using Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting;
using Tauchbolde.Common.DomainServices.PhotoStorage;
using Tauchbolde.Common.DomainServices.PhotoStorage.Stores;
using Tauchbolde.Common.DomainServices.PhotoStorage.Stores.FileSystemStore;
using Tauchbolde.Common.DomainServices.Users;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Common.DomainServices.TextFormatting;
using IImageResizer = Tauchbolde.Common.DomainServices.Avatar.IImageResizer;
using ImageResizer = Tauchbolde.Common.DomainServices.Avatar.ImageResizer;

[assembly: InternalsVisibleTo("Tauchbolde.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // For FakeItEasy to use "internal" visibility

namespace Tauchbolde.Common
{

    public static class CommonServices
    {
        public static void RegisterServices(IServiceCollection services, string photoStoreRoot)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }

            // Add application services.
            services.AddScoped<ApplicationDbContext>();
            services.AddSingleton<IMimeMapping, MimeMapping>();
            services.AddTransient<IAppEmailSender, SmtpSender>();
            services.AddSingleton<IAvatarIdGenerator, AvatarIdGenerator>();
            services.AddSingleton<IAvatarStore, AvatarStore>();
            services.AddSingleton<IImageResizer, ImageResizer>();
            services.AddScoped<ITelemetryService, AppInsightsTelemetryService>();
            services.AddScoped<ITextFormatter, MarkdownDigFormatter>();

            // Repos
            services.AddTransient<IDiverRepository, DiverRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ILogbookEntryRepository, LogbookEntryRepository>();

            // DomainServices
            services.AddTransient<IParticipationService, ParticipationService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationSender, NotificationSender>();
            services.AddTransient<INotificationTypeInfos, NotificationTypeInfos>();
            services.AddTransient<INotificationFormatter, HtmlFormatter>();
            services.AddTransient<ICssStyleFormatter, CssStyleFormatter>();
            services.AddTransient<IHtmlListFormatter, HtmlListFormatter>();
            services.AddTransient<IHtmlHeaderFormatter, HtmlHeaderFormatter>();
            services.AddTransient<IHtmlFooterFormatter, HtmlFooterFormatter>();
            services.AddTransient<IDiverService, DiversService>();
            services.AddTransient<IMassMailService, MassMailService>();
            services.AddTransient<ILogbookService, LogbookService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IPhotoStore, FilePhotoStore>();
            services.AddSingleton<IFilePhotoStoreConfiguration>(new FilePhotoStoreConfiguration(photoStoreRoot));
            services.AddTransient<IFilePhotoIdentifierSerializer, FilePhotoIdentifierSerializer>();
        }

        public static void RegisterDevelopment(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }

            services.AddTransient<INotificationSubmitter, ConsoleNotificationSubmitter>();
        }

        public static void RegisterProduction(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }

            services.AddTransient<INotificationSubmitter, SmtpNotificationSubmitter>();
        }

    }
}
