using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.EFCore.DL.Context;
using Sample.EFCore.DL.Repository.Base;

namespace Sample.EFCore.DL.Repository.Samples {
    public interface IUserRepository : IRepository<UserModel> {
    }
    public class UserRepository(DlefCoreContext context) : BaseRepository<UserModel>(context), IUserRepository {
    }

    public class UserMap : IEntityTypeConfiguration<UserModel> {
        public void Configure(EntityTypeBuilder<UserModel> builder) {
            builder
                .Property(m => m.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(m => m.GenderType);
        }
    }

    [EntityTypeConfiguration(typeof(UserMap))]
    public class UserModel : BaseEntityModel {
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

        public GenderTypeModel GenderType {
            get; set;
        }

        public List<UserGroupModel> UserGroups {
            get;
        }
    }
}