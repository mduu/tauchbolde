using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class DiverDataConfiguration : IEntityTypeConfiguration<DiverData>
    {
        public void Configure(EntityTypeBuilder<DiverData> builder)
        {
            builder.ToTable("Diver");
            
            builder.HasMany(e => e.Notificationses)
                .WithOne(e => e.Recipient)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(e => e.Comments)
                .WithOne(e => e.Author)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(e => e.Events)
                .WithOne(e => e.Organisator)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Property(e => e.NotificationIntervalInHours).HasDefaultValue(1);
            
            builder.HasOne(d => d.User);
            
            builder.HasMany(e => e.OriginalAuthorOfLogbookEntries)
                .WithOne(e => e.OriginalAuthor)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(e => e.EditorAuthorOfLogbookEntries)
                .WithOne(e => e.EditorAuthor)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}