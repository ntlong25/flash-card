using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace flash_card.business.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}
