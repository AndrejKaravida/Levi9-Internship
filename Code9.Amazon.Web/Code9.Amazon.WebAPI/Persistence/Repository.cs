using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Persistence
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _table;

        public Repository(DataContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public void Add(T obj)
        {
            _table.Add(obj);
        }

        public void Delete(T obj)
        {
            _table.Remove(obj);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _table.FindAsync(id);
        }

        public  async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
