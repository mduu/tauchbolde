using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Common.Repositories;
using Tauchbolde.DataAccess.Repositories;

[assembly: InternalsVisibleTo("Tauchbolde.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // For FakeItEasy to use "internal" visibility

namespace Tauchbolde.DataAccess
{

    public static class DataAccessServices
    {
        public static void RegisterServices(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }
        
            services.AddScoped<ApplicationDbContext>();

            // Repos
            RegisterRepositories(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IDiverRepository, DiverRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ILogbookEntryRepository, LogbookEntryRepository>();
        }
    }
}
