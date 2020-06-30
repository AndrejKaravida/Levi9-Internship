using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Helpers;
using Code9.Amazon.WebAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Persistence
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {

        public VehicleRepository(DataContext context) : base(context) { }

        public PagedList<Vehicle> GetPagedDetailed(VehicleParams vehicleParams)
        {
            var vehicles = _context
                .Vehicles
                .Include(m => m.Model)
                .ThenInclude(x => x.Make)
                .Include(f => f.Features)
                .ThenInclude(x =>x.Feature)
                .Include(c => c.Comments)
                .Include(i => i.Images)
                .AsQueryable();

            if(vehicleParams.City != null && vehicleParams.City.Length > 0)
            {
                vehicles = vehicles.Where(x => x.City.ToLower() == vehicleParams.City.ToLower());
            }

            if (vehicleParams.Make != 0 )
            {
                vehicles = vehicles.Where(x => x.Model.Make.Id == vehicleParams.Make);
            }

            if (vehicleParams.Model != 0)
            {
                vehicles = vehicles.Where(x => x.Model.Id == vehicleParams.Model);
            }

            if (vehicleParams.FuelType != null &&  vehicleParams.FuelType.Length > 0)
            {
                vehicles = vehicles.Where(x => x.FuelType.ToLower() == vehicleParams.FuelType.ToLower());
            }

            if (vehicleParams.MinPrice > 0)
            {
                vehicles = vehicles.Where(x => x.Price >= vehicleParams.MinPrice);
            }

            if (vehicleParams.MaxPrice < 999999)
            {
                vehicles = vehicles.Where(x => x.Price <= vehicleParams.MaxPrice);
            }

            if (vehicleParams.MinYear > 1950)
            {
                vehicles = vehicles.Where(x => x.ProductionYear >= vehicleParams.MinYear);
            }

            if (vehicleParams.MaxYear < 2020)
            {
                vehicles = vehicles.Where(x => x.ProductionYear <= vehicleParams.MaxYear);
            }

            if (vehicleParams.MaxMileage < 999999)
            {
                vehicles = vehicles.Where(x => x.Mileage <= vehicleParams.MaxMileage);
            }

            if (vehicleParams.OrderBy != null && vehicleParams.OrderBy.Length > 0 && vehicleParams.OrderBy != "None")
            {
                switch (vehicleParams.OrderBy)
                {
                    case "Ascending":
                        vehicles = vehicles.OrderBy(p => p.Price);
                        break;
                    default:
                        vehicles = vehicles.OrderByDescending(p => p.Price);
                        break;
                } 
            }

            return new PagedList<Vehicle>(vehicles, vehicleParams.PageNumber, vehicleParams.PageSize);
        }

        public override async Task<Vehicle> GetByIdAsync(int id)
        {
            return await _context.Vehicles
                .Include(m => m.Model)
                .ThenInclude(x => x.Make)
                .Include(f => f.Features)
                .ThenInclude(x => x.Feature)
                .Include(c => c.Comments)
                .Include(i => i.Images)
                .SingleOrDefaultAsync(v => v.Id == id);
        }
    }
}
