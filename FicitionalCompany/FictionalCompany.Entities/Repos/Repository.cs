using FictionalCompany.Entities.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;



namespace FictionalCompany.Entities.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FCDbContext _context;
        private readonly DbSet<T> _dbSet;

        private readonly IMemoryCache _cache;
        public Repository(FCDbContext context, IMemoryCache cache)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _cache = cache;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            int isUpdated = 0;
            _context.Entry(entity).State = EntityState.Modified;
            isUpdated = await _context.SaveChangesAsync();
            if (isUpdated > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            int isDeleted = 0;
            _dbSet.Remove(entity);
            isDeleted = await _context.SaveChangesAsync();
            if (isDeleted > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllWithCacheAsync(string cacheKey, TimeSpan cacheDuration)
        {
            if (_cache.TryGetValue(cacheKey, out IEnumerable<T> cachedData))
            {
                return cachedData;
            }

            var data = await _dbSet.ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration,
                SlidingExpiration = cacheDuration
            };

            _cache.Set(cacheKey, data, cacheEntryOptions);
            return data;
        }

        public async Task<T> GetByIdWithCacheAsync(int id, string cacheKey, TimeSpan cacheDuration)
        {
            if (_cache.TryGetValue(cacheKey, out IEnumerable<T> cachedData))
            {
                return cachedData.FirstOrDefault(item => (int)item.GetType().GetProperty("Id").GetValue(item) == id);
            }

            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheDuration,
                    SlidingExpiration = cacheDuration
                };

                _cache.Set(cacheKey, new List<T> { entity }, cacheEntryOptions);
            }

            return entity;
        }
    }
}


