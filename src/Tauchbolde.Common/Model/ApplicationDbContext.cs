using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tauchbolde.Common.Model
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Diver> Diver { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Diver
            builder.Entity<Diver>()
                .HasMany(e => e.Notificationses)
                .WithOne(e => e.Recipient)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Diver>()
                .HasMany(e => e.Comments)
                .WithOne(e => e.Author)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Diver>()
                .HasMany(e => e.Events)
                .WithOne(e => e.Organisator)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Diver>()
               .Property(e => e.NotificationIntervalInHours)
               .HasDefaultValue(1);
            builder.Entity<Diver>()
               .HasOne(d => d.User);

            // Comment
            builder.Entity<Comment>().HasOne(e => e.Event).WithMany(e => e.Comments).OnDelete(DeleteBehavior.Restrict);

            // Event
            builder.Entity<Event>().HasIndex(p => new { p.StartTime, p.Deleted });
            builder.Entity<Event>().Property(e => e.Deleted).HasDefaultValue(false);
            builder.Entity<Event>().Property(e => e.Canceled).HasDefaultValue(false);
            builder.Entity<Event>().HasMany(e => e.Comments).WithOne(c => c.Event);

            // Notification
            builder.Entity<Notification>().Property(e => e.AlreadySent).HasDefaultValue(false);
            builder.Entity<Notification>().Property(e => e.CountOfTries).HasDefaultValue(0);

            // Participants
            builder.Entity<Participant>().HasOne(e => e.Event).WithMany(e => e.Participants).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Participant>().Property(e => e.CountPeople).HasDefaultValue(1);

            // PostImage
        }
    }
}