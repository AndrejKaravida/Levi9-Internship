using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.IRepository.Core
{
    public interface IModelRepository : IRepository<Model>
    {
        Task<IEnumerable<Model>> GetByMakeIdAsync(int id);
    }
}
