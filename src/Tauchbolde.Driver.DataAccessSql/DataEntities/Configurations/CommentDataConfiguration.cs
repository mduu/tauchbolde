using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class CommentDataConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            
            builder.HasOne(e => e.Event)
                .WithMany(e => e.Comments)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}