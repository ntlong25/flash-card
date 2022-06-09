using flash_card.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using flash_card.data.Entities;

namespace flash_card.business.Repository.Implement
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _dataContext;
        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return _dataContext.Set<T>().Where(expression);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAsync()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dataContext.Set<T>().Update(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            T entity = await _dataContext.Set<T>().FindAsync(id);
            _dataContext.Set<T>().Remove(entity);
        }
    }
}
