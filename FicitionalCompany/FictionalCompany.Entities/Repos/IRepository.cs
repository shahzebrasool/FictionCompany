using System.Linq.Expressions;

namespace FictionalCompany.Entities.Repos
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

        // Methods for caching
        Task<IEnumerable<T>> GetAllWithCacheAsync(string cacheKey, TimeSpan cacheDuration);
        Task<T> GetByIdWithCacheAsync(int id, string cacheKey, TimeSpan cacheDuration);

    }
}
