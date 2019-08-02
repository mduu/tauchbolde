using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class LogbookEntryDataConfiguration : IEntityTypeConfiguration<LogbookEntry>
    {
        public void Configure(EntityTypeBuilder<LogbookEntry> builder)
        {
            builder.ToTable("LogbookEntries");
            
            builder.HasIndex(e => new { e.IsFavorite, e.CreatedAt});

        }
    }
}