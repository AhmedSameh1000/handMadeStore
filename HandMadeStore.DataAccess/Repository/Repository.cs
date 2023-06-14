using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HandMadeStore.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T Entity)
        {
            _dbSet.Add(Entity);
        }

        public List<T> GetAll(Expression<Func<T, bool>> Filter = null)
        {
            IQueryable<T> qeury = _dbSet.AsQueryable();
            if (Filter != null) qeury = qeury.Where(Filter);
            return qeury.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> Filter)
        {
            IQueryable<T> qeury = _dbSet.AsQueryable();
            qeury = qeury.Where(Filter);
            return qeury.FirstOrDefault();
        }

        public void Remove(T Entity)
        {
            _dbSet.Remove(Entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}