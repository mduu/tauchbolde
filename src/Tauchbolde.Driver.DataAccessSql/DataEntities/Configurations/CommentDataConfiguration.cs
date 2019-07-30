using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class CommentDataConfiguration : IEntityTypeConfiguration<CommentData>
    {
        public void Configure(EntityTypeBuilder<CommentData> builder)
        {
            builder.ToTable("Comments");
            
            builder.HasOne(e => e.Event)
                .WithMany(e => e.Comments)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}