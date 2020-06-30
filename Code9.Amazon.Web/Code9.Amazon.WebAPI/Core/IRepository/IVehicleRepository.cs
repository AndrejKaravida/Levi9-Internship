using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Helpers;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IRepository
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        PagedList<Vehicle> GetPagedDetailed(VehicleParams vehicleParams);
    }
}
