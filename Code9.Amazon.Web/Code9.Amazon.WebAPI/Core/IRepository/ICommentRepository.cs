using Code9.Amazon.WebAPI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {

        Task<IEnumerable<Comment>> GetAllForVehicle(int vehicleId);

    }
}
