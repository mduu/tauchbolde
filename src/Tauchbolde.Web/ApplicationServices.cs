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
        public static void Register(Startup instance, IServiceCollection services, bool isDevelopment)
        {
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddScoped<ApplicationDbContext>();

            // Repos
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IPostImageRepository, PostImageRepository>();

            // DomainServices
            services.AddTransient<IParticipationService, ParticipationService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationSender, NotificationSender>();
            services.AddTransient<INotificationFormatter, HtmlNotificationFormatter>();

            if (isDevelopment)
            {
                services.AddTransient<INotificationSubmitter, ConsoleNotificationSubmitter>();
            }
            else
            {
                services.AddTransient<INotificationSubmitter, SmtpNotificationSubmitter>();
            }
        }

    }
}
