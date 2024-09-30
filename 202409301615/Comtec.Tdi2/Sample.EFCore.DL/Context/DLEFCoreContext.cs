using Microsoft.EntityFrameworkCore;
using Sample.EFCore.DL.Repository.Samples;

namespace Sample.EFCore.DL.Context {
    public class DlefCoreContext : DbContext {
        // members
        public DbSet<EntityIdModel> EntityId {
            get; set;
        }
        public DbSet<EntityNoIdModel> EntityNoId {
            get; set;
        }
        public DbSet<GenderTypeModel> GenderType {
            get; set;
        }
        public DbSet<DataTypesModel> DataTypes {
            get; set;
        }

        public DbSet<UserModel> Users {
            get; set;
        }
        public DbSet<GroupModel> Groups {
            get; set;
        }
        public DbSet<UserGroupModel> UsersGroups {
            get; set;
        }

        // class
        public DlefCoreContext() {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        // events
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFCore2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}