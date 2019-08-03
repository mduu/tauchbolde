using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Driver.DataAccessSql.EntityConfigurations
{
    public class EventDataConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");
            
            builder.HasIndex(p => new { p.StartTime, p.Deleted });
            builder.Property(e => e.Deleted).HasDefaultValue(false);
            builder.Property(e => e.Canceled).HasDefaultValue(false);
            builder.HasMany(e => e.Comments).WithOne(c => c.Event);
        }
    }
}