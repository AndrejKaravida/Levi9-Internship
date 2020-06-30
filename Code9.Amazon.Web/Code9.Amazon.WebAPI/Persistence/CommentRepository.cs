using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Persistence
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {

        public CommentRepository(DataContext context) : base(context){}


        public async Task<IEnumerable<Comment>> GetAllForVehicle(int vehicleId)
        {
            return await _context.Comments.Include(u => u.User).Where(x => x.VehicleId == vehicleId).ToListAsync();
        }

    }
}
