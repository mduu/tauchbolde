﻿using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Driver.DataAccessSql.Repositories;


namespace Tauchbolde.Driver.DataAccessSql
{

    public static class DataAccessServices
    {
        public static void RegisterServices(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }
        
            services.AddScoped<ApplicationDbContext>();
            
            services.AddTransient<IDiverRepository, DiverRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ILogbookEntryRepository, LogbookEntryRepository>();
        }
    }
}
