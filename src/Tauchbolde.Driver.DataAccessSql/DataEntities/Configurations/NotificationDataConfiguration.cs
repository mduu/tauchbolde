using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class NotificationDataConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            
            builder.Property(e => e.AlreadySent).HasDefaultValue(false);
            builder.Property(e => e.CountOfTries).HasDefaultValue(0);
        }
    }
}