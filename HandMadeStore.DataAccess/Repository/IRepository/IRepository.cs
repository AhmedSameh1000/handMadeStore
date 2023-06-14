using System.Linq.Expressions;

namespace HandMadeStore.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll(Expression<Func<T, bool>> Filter = null);

        void Add(T Entity);

        T GetFirstOrDefault(Expression<Func<T, bool>> Filter);

        void Remove(T Entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}