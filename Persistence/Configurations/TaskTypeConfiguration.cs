using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPC.Api.Model.Lookups;

namespace TPC.Api.Persistence.Configurations
{
    public static class TaskTypeConfiguration
    {
        public static void Configure(this EntityTypeBuilder<TaskType> builder)
        {
            builder.ToTable("TaskTypes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.Name).HasColumnName("TypeName");
        }
    }
}
