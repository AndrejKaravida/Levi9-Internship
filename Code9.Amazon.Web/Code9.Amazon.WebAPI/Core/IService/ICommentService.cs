using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IService
{
    public interface ICommentService : IService<Comment>
    {
        Task<IEnumerable<Comment>> GetAllForVehicleAsync(int vehicleId);
    }
}
