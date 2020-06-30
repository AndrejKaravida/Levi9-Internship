using Code9.Amazon.WebAPI.Core.Models;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IService
{
    public interface IImageService
    {
        Task AddAsync(Image obj);
        Task DeleteAsync(Image obj);
        Task<Image> GetByIdAsync(int id);
        Task<Image> GetMainImageForVehicle(int vehicleId);
        Task SetMainPhoto(int vehicleId, int photoId);
    }
}
