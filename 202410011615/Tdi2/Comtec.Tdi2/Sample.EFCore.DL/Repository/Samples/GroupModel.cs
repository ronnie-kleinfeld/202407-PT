using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.EFCore.DL.Context;
using Sample.EFCore.DL.Repository.Base;

namespace Sample.EFCore.DL.Repository.Samples {
    public interface IGroupRepository : IRepository<GroupModel> {
    }
    public class GroupRepository(DlefCoreContext context) : BaseRepository<GroupModel>(context), IGroupRepository {
    }

    public class GroupMap : IEntityTypeConfiguration<GroupModel> {
        public void Configure(EntityTypeBuilder<GroupModel> builder) {
            builder
                .Property(m => m.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");
        }
    }

    [EntityTypeConfiguration(typeof(GroupMap))]
    public class GroupModel : BaseEntityModel {
        public int Id {
            get; set;
        }

        public DateTime CreatedDateTime {
            get; set;
        }

        [MaxLength(100)]
        public string GroupName {
            get; set;
        }

        public List<UserGroupModel> GroupUsers {
            get;
        }
    }
}