using SiggaTechAPP.Interfaces;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SiggaTechAPP.Repositories
{
    public class SQLiteRetositoryBase<TEntity> : ISQLiteRetositoryBase<TEntity> where TEntity : class, new()
    {
        private readonly SQLiteAsyncConnection _sqliteConnection;

        public SQLiteRetositoryBase()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            _sqliteConnection = new SQLiteAsyncConnection(Path.Combine(config.DatabaseSQLitePath, "SiggaTech.db3"));
        }

        public async Task<List<TEntity>> GetAllItemsAsync<TValue>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, TValue>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return await _sqliteConnection.GetAllWithChildrenAsync<TEntity>(predicate);
        }

        public async Task<TEntity> GetByIdAsync(int id) => await _sqliteConnection.FindAsync<TEntity>(id);

        public async Task<TEntity> GetItemAsync<TValue>(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _sqliteConnection.Table<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task AddItemAsync(TEntity entity)
        {
            await _sqliteConnection.InsertAsync(entity);
        }

        public async Task UpdateItemAsync(TEntity entity, int id)
        {
            await _sqliteConnection.UpdateAsync(entity);
        }

        public async Task RemoveItemAsync(TEntity entity)
        {
            await _sqliteConnection.DeleteAsync(entity);
        }

        public void Dispose()
        {
            //_sqliteConnection?.Dispose();
        }
    }
}
