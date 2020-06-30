using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repo;

        public Service(IRepository<T> repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(T obj)
        {
            _repo.Add(obj);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(T obj)
        {
            _repo.Delete(obj);
            await SaveAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _repo.SaveAsync();
        }
    }
}
