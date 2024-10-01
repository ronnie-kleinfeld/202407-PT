using Comtec.DL.Context;
using Microsoft.EntityFrameworkCore;

namespace Comtec.AS400XML.DL.Context {
    public class AS400XmlContext : BaseContext {
        // members
        public DbSet<XmlFileModel> XmlFiles {
            get; set;
        }

        // events
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFCore2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}