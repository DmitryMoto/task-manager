using ConsoleTaskManager.Data;
using ConsoleTaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleTaskManager.Services
{
    internal class GenericService<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericService(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync(params string[] includes)
        {
            var query = _dbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Объект с ID {id} не найден.");
            }
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

    internal class AppTaskService : GenericService<AppTask>
    {
        public AppTaskService(AppDbContext context) : base(context)
        {
        }
    }

    internal class PeopleService : GenericService<People>
    {
        public PeopleService(AppDbContext context) : base(context)
        {
        }
    }
}
