using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tauchbolde.Common.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // ApplicationUser
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Notificationses)
                .WithOne(e => e.Recipient)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Comments)
                .WithOne(e => e.Author)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Events)
                .WithOne(e => e.Organisator)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Posts)
                .WithOne(e => e.Author)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment
            builder.Entity<Comment>().HasOne(e => e.Event).WithMany(e => e.Comments).OnDelete(DeleteBehavior.Restrict);

            // Event
            builder.Entity<Event>().HasIndex(p => new { p.StartTime, p.Deleted });
            builder.Entity<Event>().Property(e => e.Deleted).HasDefaultValue(false);
            builder.Entity<Event>().Property(e => e.Canceled).HasDefaultValue(false);

            // Notification
            builder.Entity<Notifications>().Property(e => e.AlreadySent).HasDefaultValue(false);
            builder.Entity<Notifications>().Property(e => e.CountOfTries).HasDefaultValue(0);

            // Participants
            builder.Entity<Participant>().HasOne(e => e.Event).WithMany(e => e.Participants).OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<Participant>().HasIndex(p => new { p.EventId, p.User }).IsUnique();
            builder.Entity<Participant>().Property(e => e.CountPeople).HasDefaultValue(1);

            // Post
            builder.Entity<Post>().HasIndex(p => new { p.Category, p.PublishDate });

            // PostImage

            // UserInfo
            builder.Entity<UserInfo>().Property(e => e.NotificationIntervalInHours).HasDefaultValue(1);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

    }
}