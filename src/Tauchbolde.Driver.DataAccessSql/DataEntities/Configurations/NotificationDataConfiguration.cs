using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class NotificationDataConfiguration : IEntityTypeConfiguration<NotificationData>
    {
        public void Configure(EntityTypeBuilder<NotificationData> builder)
        {
            builder.ToTable("Notifications");
            
            builder.Property(e => e.AlreadySent).HasDefaultValue(false);
            builder.Property(e => e.CountOfTries).HasDefaultValue(0);
        }
    }
}