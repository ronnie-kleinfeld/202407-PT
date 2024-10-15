using System.Linq.Expressions;
using Comtec.DL.Model;

namespace Comtec.DL.Repository.Base {
    public interface IRepository<T> where T : BaseEntityModel {
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        T SingleOrDefault() {
            return SingleOrDefault(null);
        }

        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}