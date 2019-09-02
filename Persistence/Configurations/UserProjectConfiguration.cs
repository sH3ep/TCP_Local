using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPC.Api.Model.ManyToManyRelations;


namespace TPC.Api.Persistence.Configurations
{
    public static class UserProjectConfiguration
    {
        public static void Configure(this EntityTypeBuilder<UserProject> builder)
        {
            builder.ToTable("User_Project");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();

            builder.Property(x => x.ProjectId).HasColumnName("ProjectId").IsRequired();
            builder.Property(x => x.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(x => x.Active).HasColumnName("Active").IsRequired();
            builder.Property(x => x.RoleId).HasColumnName("RoleId").IsRequired();

        }
    }
}
