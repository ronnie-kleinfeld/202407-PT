using Sample.EFCore.DL.Context;
using System.Linq.Expressions;

namespace Sample.EFCore.DL.Repository.Base {
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntityModel {
        public DlefCoreContext Context {
            get; set;
        }

        public BaseRepository(DlefCoreContext context) {
            Context = context;
        }

        public T Get(int id) {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return Context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll() {
            // Note that here I've repeated Context.Set<TEntity>() in every method and this is causing
            // too much noise. I could get a reference to the DbSet returned from this method in the 
            // constructor and store it in a private field like _entities. This way, the implementation
            // of our methods would be cleaner:
            // 
            // _entities.ToList();
            // _entities.Where();
            // _entities.SingleOrDefault();
            // 
            // I dIdn't change it because I wanted the code to look like the vIdeos. But feel free to change
            // this on your own.
            return Context.Set<T>().ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) {
            return Context.Set<T>().Where(predicate);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            return Context.Set<T>().SingleOrDefault(predicate);
        }

        public void Add(T entity) {
            Context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities) {
            foreach (T tEntity in entities) {
            }
            Context.Set<T>().AddRange(entities);
        }

        public void Remove(T entity) {
            Context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities) {
            Context.Set<T>().RemoveRange(entities);
        }
    }
}