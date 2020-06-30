using Code9.Amazon.WebAPI.Core;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.IRepository.Core;
using Code9.Amazon.WebAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Persistence
{
    public class ModelRepository : Repository<Model>, IModelRepository
    {

        public ModelRepository(DataContext context) : base(context){ }

        public async Task<IEnumerable<Model>> GetByMakeIdAsync(int id)
        {
            return await _context.Models.Where(m => m.MakeId == id).ToListAsync();
        }
    }
}
