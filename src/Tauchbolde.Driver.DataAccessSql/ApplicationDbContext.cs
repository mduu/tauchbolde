using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Driver.DataAccessSql
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IMediator mediator;

        public ApplicationDbContext([NotNull] IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            [NotNull] IMediator mediator)
        : base(options)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public DbSet<EventData> Events { get; set; }
        public DbSet<ParticipantData> Participants { get; set; }
        public DbSet<CommentData> Comments { get; set; }
        public DbSet<DiverData> Diver { get; set; }
        public DbSet<NotificationData> Notifications { get; set; }
        public DbSet<LogbookEntryData> LogbookEntries { get; set; }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            DispatchDomainEventsForSuccessfullySavedEntities();
            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            DispatchDomainEventsForSuccessfullySavedEntities();
            return result;
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfiguration(new CommentDataConfiguration());
            builder.ApplyConfiguration(new DiverDataConfiguration());
            builder.ApplyConfiguration(new EventDataConfiguration());
            builder.ApplyConfiguration(new LogbookEntryDataConfiguration());
            builder.ApplyConfiguration(new NotificationDataConfiguration());
            builder.ApplyConfiguration(new ParticipantDataConfiguration());
        }
        
        private void DispatchDomainEventsForSuccessfullySavedEntities()
        {
            var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
                .Select(e => e.Entity)
                .Where(e => e.UncommittedDomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.UncommittedDomainEvents.ToArray();
                entity.UncommittedDomainEvents.Clear();
                foreach (var domainEvent in events)
                {
                    mediator.Publish(domainEvent);
                }
            }
        }
    }
}