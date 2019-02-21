using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Web.Services;
using Tauchbolde.Web.Core;
using Tauchbolde.Common.DomainServices.Avatar;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Tauchbolde.Common;

namespace Tauchbolde.Web
{
    public class ApplicationServices
    {
        public static void Register(IServiceCollection services)
        {
            CommonServices.RegisterServices(services);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlGenerator, MvcUrlGenerator>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IAvatarPathProvider, AvatarPathProvider>();
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
