using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using Code9.Amazon.WebAPI.Helpers;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IService
{
    public interface IVehicleService : IService<Vehicle>
    {
        PagedList<Vehicle> GetPagedDetailed(VehicleParams vehicleParams);

    }
}
