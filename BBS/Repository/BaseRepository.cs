
namespace BBS.Repository
{
    using BBS.Data;
    using BBS.IRepository;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await _context.Set<T>().Where(whereLambda).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(int page, int size, Expression<Func<T, bool>> whereLambda)
        {
            return await _context.Set<T>().Where(whereLambda).Skip((page - 1) * size).Take(size).ToListAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(whereLambda);
        }
        public async Task<bool> IsExist(Expression<Func<T, bool>> whereLambda)
        {
            return _context.Set<T>().Any(whereLambda);
        }
    }
}