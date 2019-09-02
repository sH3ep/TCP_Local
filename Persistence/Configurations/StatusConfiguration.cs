using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPC.Api.Model.Lookups;

namespace TPC.Api.Persistence.Configurations
{
    public static class StatusConfiguration
    {
        public static void Configure(this EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Statuses");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.Name).HasColumnName("StatusName");
        }
    }
}
