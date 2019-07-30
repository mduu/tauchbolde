using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class LogbookEntryDataConfiguration : IEntityTypeConfiguration<LogbookEntryData>
    {
        public void Configure(EntityTypeBuilder<LogbookEntryData> builder)
        {
            builder.ToTable("LogbookEntries");
            
            builder.HasIndex(e => new { e.IsFavorite, e.CreatedAt});

        }
    }
}