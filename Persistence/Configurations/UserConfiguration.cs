using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TPC.Api.Model;

namespace TPC.Api.Persistence.Configurations
{
    public static class UserConfiguration
    {
        public static void Configure(this EntityTypeBuilder<User> builder)
        {
            var converter =new BytesToStringConverter();

            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.FirstName).HasColumnName("FirstName");
            builder.Property(x => x.LastName).HasColumnName("LastName");
            builder.Property(x => x.Email).HasColumnName("Email");
            builder.Property(x => x.Activated).HasColumnName("Activated");
            builder.Property(x => x.PasswordHash).HasColumnName("PasswordHash").HasConversion(converter);
            builder.Property(x => x.PasswordSalt).HasColumnName("PasswordSalt").HasConversion(converter);
        }
    }
}
