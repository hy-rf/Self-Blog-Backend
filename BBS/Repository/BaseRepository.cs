
namespace BBS.Repository{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BBS.Data;
    using BBS.IRepository;
    using BBS.Models;
    using Microsoft.EntityFrameworkCore;

    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await _dbSet.Where(whereLambda).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(int page, int size, Expression<Func<T, bool>> whereLambda)
        {
            return await _dbSet.Where(whereLambda).Skip((page - 1) * size).Take(size).ToListAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> whereLambda){
            return await _dbSet.FirstOrDefaultAsync(whereLambda);
        }
        public async Task<bool> IsExist(Expression<Func<T, bool>> whereLambda){
            return _dbSet.Any(whereLambda);
        }
    }
}