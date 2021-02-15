using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nimb3s.Data.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The repository type</typeparam>
    /// <typeparam name="K">The data type of key the repository uses for its Id</typeparam>
    public interface IRepository<TEntity, TKey>
    {
        Task<TEntity> GetAsync(TKey Id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task UpsertAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
