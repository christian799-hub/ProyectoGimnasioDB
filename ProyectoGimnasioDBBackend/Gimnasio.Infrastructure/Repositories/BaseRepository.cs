
using Gimnasio.Core.Entities;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Core.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly GimnasioContext _context;
        protected readonly DbSet<T> _entities;
        public BaseRepository(GimnasioContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            
        }

        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
            
        }
        public async Task DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}