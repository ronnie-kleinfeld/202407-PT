using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.EFCore.DL.Context;
using Sample.EFCore.DL.Repository.Base;

namespace Sample.EFCore.DL.Repository.Samples {
    public interface IEntityNoIdRepository : IRepository<EntityNoIdModel> {
    }
    public class EntityNoIdRepository(DlefCoreContext context) : BaseRepository<EntityNoIdModel>(context), IEntityNoIdRepository {
    }

    public class EntityNoIdMap : IEntityTypeConfiguration<EntityNoIdModel> {
        public void Configure(EntityTypeBuilder<EntityNoIdModel> builder) {
            builder
                .HasNoKey();

            builder
                .Property(m => m.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");
        }
    }

    [EntityTypeConfiguration(typeof(EntityNoIdMap))]
    public class EntityNoIdModel : BaseEntityModel {
        public DateTime CreatedDateTime {
            get; set;
        }
    }
}