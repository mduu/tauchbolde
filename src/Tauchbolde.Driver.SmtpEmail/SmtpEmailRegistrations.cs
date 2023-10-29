using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Application.Services.Notifications;

namespace Tauchbolde.Driver.SmtpEmail
{
    public static class SmtpEmailRegistrations
    {
        public static void RegisterServices(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddTransient<IAppEmailSender, SmtpSender>();
            services.AddTransient<INotificationSubmitter, SmtpNotificationSubmitter>();
        }
    }
}