using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.IRepository.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Services
{
    public class ModelService : Service<Model>, IModelService
    {
        private readonly IModelRepository _repos;


        public ModelService(IModelRepository repo) : base(repo)
        {
            _repos = repo;
        }


        public async Task<IEnumerable<Model>> GetByMakeIdAsync(int id)
        {
            return await _repos.GetByMakeIdAsync(id);
        }

    }
}
