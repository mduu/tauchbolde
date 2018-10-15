using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Services;

namespace Tauchbolde.Web
{
    public class ApplicationServices
    {
        public static void Register(IServiceCollection services)
        {
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddScoped<ApplicationDbContext>();

            // Repos
            services.AddTransient<IDiverRepository, DiverRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();

            // DomainServices
            services.AddTransient<IParticipationService, ParticipationService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationSender, NotificationSender>();
            services.AddTransient<INotificationFormatter, HtmlNotificationFormatter>();
            services.AddTransient<IDiverService, DiversService>();
        }

        public static void RegisterDevelopment(IServiceCollection services)
        {
            if (services == null) { throw new System.ArgumentNullException(nameof(services)); }

            services.AddTransient<INotificationSubmitter, ConsoleNotificationSubmitter>();
        }

        public static void RegisterProduction(IServiceCollection services)
        {
            if (services == null) { throw new System.ArgumentNullException(nameof(services)); }

            services.AddTransient<INotificationSubmitter, SmtpNotificationSubmitter>();
        }


    }
}
