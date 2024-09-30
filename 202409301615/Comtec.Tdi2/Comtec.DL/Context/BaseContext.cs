using Comtec.BE.Config;
using Microsoft.EntityFrameworkCore;

namespace Comtec.DL.Context {
    public abstract class BaseContext : DbContext {
        // members
        //public DbSet<UserModel> Users {
        //    get; set;
        //}

        // events
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(AppConfigHandler.Data.DL.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}