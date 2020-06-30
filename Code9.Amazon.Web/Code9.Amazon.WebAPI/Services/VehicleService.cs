using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using Code9.Amazon.WebAPI.Helpers;
using System;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Services
{
    public class VehicleService : Service<Vehicle>, IVehicleService
    {
        private readonly IVehicleRepository _repos;

        public VehicleService(IVehicleRepository repo) : base(repo)
        {
            _repos = repo;
        }

        public PagedList<Vehicle> GetPagedDetailed(VehicleParams vehicleParams)
        {
            return _repos.GetPagedDetailed(vehicleParams);
        }

    }
}
