using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPC.Api.Model;

namespace TPC.Api.Persistence.Configurations
{
    public static class SprintConfiguration
    {
        public static void Configure(this EntityTypeBuilder<Sprint> builder)
        {
            builder.ToTable("Sprints");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
            builder.Property(x => x.ModificationDate).HasColumnName("ModificationDate");
            builder.Property(x => x.ProjectId).HasColumnName("ProjectId");
            builder.Property(x => x.SprintName).HasColumnName("SprintName");
            builder.Property(x => x.StartDate).HasColumnName("StartDate");
            builder.Property(x => x.EndDate).HasColumnName("EndDate");
        }
    }
}
