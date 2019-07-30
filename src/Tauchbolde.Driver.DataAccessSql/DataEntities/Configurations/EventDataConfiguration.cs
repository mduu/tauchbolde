using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class EventDataConfiguration : IEntityTypeConfiguration<EventData>
    {
        public void Configure(EntityTypeBuilder<EventData> builder)
        {
            builder.ToTable("Events");
            
            builder.HasIndex(p => new { p.StartTime, p.Deleted });
            builder.Property(e => e.Deleted).HasDefaultValue(false);
            builder.Property(e => e.Canceled).HasDefaultValue(false);
            builder.HasMany(e => e.Comments).WithOne(c => c.Event);
        }
    }
}