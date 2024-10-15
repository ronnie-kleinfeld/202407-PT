using Comtec.AS400XML.Output.Model;
using Comtec.AS400XML.Output.Model.Screen;
using Comtec.BE.Config;
using Comtec.DL.Context;
using Microsoft.EntityFrameworkCore;

namespace Comtec.AS400XML.Output.Context {
    public class AS400XmlContext : BaseContext {
        // members
        public DbSet<AS400XmlOutputModel> AS400XmlOutput {
            get; set;
        }

        public DbSet<ArXmlElement> ArXmlElement {
            get; set;
        }
        public DbSet<BgrXmlElement> BgrXmlElement {
            get; set;
        }
        public DbSet<BXmlElement> BXmlElement {
            get; set;
        }
        public DbSet<CmdXmlElement> CmdXmlElement {
            get; set;
        }
        public DbSet<CXmlElement> CXmlElement {
            get; set;
        }
        public DbSet<DXmlElement> DXmlElement {
            get; set;
        }
        public DbSet<FieldsXmlElement> FieldsXmlElement {
            get; set;
        }
        public DbSet<FolderDetailsXmlElement> FolderDetailsXmlElement {
            get; set;
        }
        public DbSet<FolderXmlElement> FolderXmlElement {
            get; set;
        }
        public DbSet<FXmlElement> FXmlElement {
            get; set;
        }
        public DbSet<LXmlElement> LXmlElement {
            get; set;
        }
        public DbSet<ScreenOutputXmlRoot> ScreenOutputXmlRoot {
            get; set;
        }
        public DbSet<RctBkgXmlElement> RctBkgXmlElement {
            get; set;
        }
        public DbSet<RctXmlElement> RctXmlElement {
            get; set;
        }
        public DbSet<RXmlElement> RXmlElement {
            get; set;
        }
        public DbSet<SXmlElement> SXmlElement {
            get; set;
        }
        public DbSet<XXmlElement> XXmlElement {
            get; set;
        }
        public DbSet<XZonesBkgXmlElement> XZonesBkgXmlElement {
            get; set;
        }
        public DbSet<XZonesXmlElement> XZonesXmlElement {
            get; set;
        }
        public DbSet<YXmlElement> YXmlElement {
            get; set;
        }
        public DbSet<YZonesBkgXmlElement> YZonesBkgXmlElement {
            get; set;
        }
        public DbSet<YZonesXmlElement> YZonesXmlElement {
            get; set;
        }
        public DbSet<ZXmlElement> ZXmlElement {
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