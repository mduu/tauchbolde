using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
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
            
            // TODO Extract the following mapping into their own files

            // Diver
            builder.Entity<DiverData>()
                .HasMany(e => e.Notificationses)
                .WithOne(e => e.Recipient)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<DiverData>()
                .HasMany(e => e.Comments)
                .WithOne(e => e.Author)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<DiverData>()
                .HasMany(e => e.Events)
                .WithOne(e => e.Organisator)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<DiverData>()
               .Property(e => e.NotificationIntervalInHours)
               .HasDefaultValue(1);
            builder.Entity<DiverData>()
               .HasOne(d => d.User);
            builder.Entity<DiverData>()
                .HasMany(e => e.OriginalAuthorOfLogbookEntries)
                .WithOne(e => e.OriginalAuthor)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<DiverData>()
                .HasMany(e => e.EditorAuthorOfLogbookEntries)
                .WithOne(e => e.EditorAuthor)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment
            builder.Entity<CommentData>().HasOne(e => e.Event).WithMany(e => e.Comments).OnDelete(DeleteBehavior.Restrict);

            // Event
            builder.Entity<DataEntities.EventData>().HasIndex(p => new { p.StartTime, p.Deleted });
            builder.Entity<DataEntities.EventData>().Property(e => e.Deleted).HasDefaultValue(false);
            builder.Entity<DataEntities.EventData>().Property(e => e.Canceled).HasDefaultValue(false);
            builder.Entity<DataEntities.EventData>().HasMany(e => e.Comments).WithOne(c => c.Event);

            // Notification
            builder.Entity<NotificationData>().Property(e => e.AlreadySent).HasDefaultValue(false);
            builder.Entity<NotificationData>().Property(e => e.CountOfTries).HasDefaultValue(0);

            // Participants
            builder.Entity<ParticipantData>().HasOne(e => e.Event).WithMany(e => e.Participants).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ParticipantData>().Property(e => e.CountPeople).HasDefaultValue(1);

            // LogbookEntry
            builder.Entity<LogbookEntryData>().HasIndex(e => new { e.IsFavorite, e.CreatedAt});
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