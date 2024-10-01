using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.EFCore.DL.Context;
using Sample.EFCore.DL.Repository.Base;

namespace Sample.EFCore.DL.Repository.Samples {
    public interface IEntityIdRepository : IRepository<EntityIdModel> {
    }
    public class EntityIdRepository(DlefCoreContext context) : BaseRepository<EntityIdModel>(context), IEntityIdRepository {
    }

    public class EntityIdMap : IEntityTypeConfiguration<EntityIdModel> {
        public void Configure(EntityTypeBuilder<EntityIdModel> builder) {
            builder
                .Property(m => m.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");
        }
    }

    [EntityTypeConfiguration(typeof(EntityIdMap))]
    public class EntityIdModel : BaseEntityModel {
        public int Id {
            get; set;
        }

        public DateTime CreatedDateTime {
            get; set;
        }
    }
}