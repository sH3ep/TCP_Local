using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Model.Lookups;
using TPC.Api.Model.ManyToManyRelations;
using TPC.Api.Persistence.Configurations;

namespace TPC.Api.Persistence
{
    public class TcpContext : DbContext
    {
        public TcpContext(DbContextOptions<TcpContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=tcp:tpcdbserver.database.windows.net,1433;Initial Catalog=tpcdb;Persist Security Info=False;User ID=tpcadmin;Password=MySuperPass1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Priority>().Configure();
            modelBuilder.Entity<Role>().Configure();
            modelBuilder.Entity<Status>().Configure();
            modelBuilder.Entity<TaskType>().Configure();
            modelBuilder.Entity<Comment>().Configure();
            modelBuilder.Entity<Feature>().Configure();
            modelBuilder.Entity<Project>().Configure();
            modelBuilder.Entity<Sprint>().Configure();
            modelBuilder.Entity<TaskItem>().Configure();
            modelBuilder.Entity<User>().Configure();
            modelBuilder.Entity<UserStory>().Configure();
            modelBuilder.Entity<UserProject>().Configure();


        }


    }

    
}
