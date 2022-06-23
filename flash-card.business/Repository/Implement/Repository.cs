using flash_card.data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace flash_card.business.Repository.Implement
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _dataContext;
        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return _dataContext.Set<T>().Where(expression);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dataContext.Set<T>().Update(entity);
            return await Task.FromResult(entity);
        }
    }
}
