using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal readonly BookingPlatformDbContext _dbContext;

        public Repository(BookingPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
           return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            T? result = await _dbContext.Set<T>().FindAsync(id);
            return result;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
           _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteAsync(T entity)
        {
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _dbContext.Set<T>().Remove(entity);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
