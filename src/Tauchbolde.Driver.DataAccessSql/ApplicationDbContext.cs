using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.EntityConfigurations;
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

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Diver> Diver { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<LogbookEntry> LogbookEntries { get; set; }

        public override int SaveChanges()
        {
            throw new InvalidOperationException("Use only the SaveChangesAsync() because synchronous saving is not supported!");
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await DispatchDomainEventsForSuccessfullySavedEntities();
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
        
        private async Task DispatchDomainEventsForSuccessfullySavedEntities()
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
                    await mediator.Publish(domainEvent);
                }
            }
        }
    }
}