

using System.Linq.Expressions;

namespace BBS.IRepository
{
    public interface IBaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task CreateAsync(T entity);
        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetAsync(int id);
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAsync();
        /// <summary>
        /// Get all entities by condition
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// Get all entities by page and size
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(int page, int size, Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);
        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
        Task<T> GetOneAsync(Expression<Func<T, bool>> whereLambda);
        Task<bool> IsExist(Expression<Func<T, bool>> whereLambda);
    }
}
