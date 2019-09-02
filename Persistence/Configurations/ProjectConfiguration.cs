using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPC.Api.Model;

namespace TPC.Api.Persistence.Configurations
{
    public static class ProjectConfiguration
    {
        public static void Configure(this EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
            builder.Property(x => x.ModificationDate).HasColumnName("ModificationDate");
            builder.Property(x => x.Title).HasColumnName("Title");
        }
    }
}
