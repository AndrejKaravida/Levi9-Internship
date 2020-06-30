using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IService
{
    public interface IModelService : IService<Model>
    {
        Task<IEnumerable<Model>> GetByMakeIdAsync(int id);
    }
}
