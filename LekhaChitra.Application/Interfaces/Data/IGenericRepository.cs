using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Interfaces.Data
{
    public interface IGenericRepository<T>
        where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByPrimaryKey<TPrimaryKey>(TPrimaryKey id);

        Task<bool> DoesExists(Expression<Func<T, bool>> filter);

        Task<T> GetSingleBySpec(Expression<Func<T, bool>> filter);
        IQueryable<T> GetQueryable();
        Task<IEnumerable<T>> GetListBySpec(Expression<Func<T, bool>> filter);

        Task<IEnumerable<T>> GetWithInclude(Expression<Func<T, object>>[] children);

        
      
    }
}
