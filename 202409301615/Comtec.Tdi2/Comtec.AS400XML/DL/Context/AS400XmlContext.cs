using Comtec.AS400XML.Output.Model;
using Comtec.BE.Config;
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

            optionsBuilder.UseSqlServer(AppConfigHandler.Data.AS400Xml.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}