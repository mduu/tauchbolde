using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities.Configurations
{
    public class ParticipantDataConfiguration : IEntityTypeConfiguration<ParticipantData>
    {
        public void Configure(EntityTypeBuilder<ParticipantData> builder)
        {
            builder.ToTable("Participants");
            
            builder.HasOne(e => e.Event).WithMany(e => e.Participants).OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.CountPeople).HasDefaultValue(1); 

        }
    }
}