using Code9.Amazon.WebAPI.Core.Models;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IRepository
{
    public interface IImageRepository
    {
        void Add(Image obj);
        void Delete(Image obj);
        Task<bool> SaveAsync();
        Task<Image> GetByIdAsync(int id);
        Task<Image> GetMainImageForVehicle(int vehicleId);
    }
}
