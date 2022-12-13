using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SiggaTechAPP.Interfaces
{
    public interface ISQLiteRetositoryBase<TEntity> : IDisposable where TEntity : class, new()
    {
        Task<List<TEntity>> GetAllItemsAsync<TValue>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, TValue>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetItemAsync<TValue>(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> GetByIdAsync(int id);
        Task AddItemAsync(TEntity entity);
        Task UpdateItemAsync(TEntity entity, int id);
        Task RemoveItemAsync(TEntity entity);
    }
}
