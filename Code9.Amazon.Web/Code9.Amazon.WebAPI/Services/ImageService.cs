using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Services
{
    public class ImageService : IImageService
    {

        private readonly IImageRepository _repo;


        public ImageService(IImageRepository repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(Image obj)
        {
            _repo.Add(obj);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(Image obj)
        {
            _repo.Delete(obj);
            await _repo.SaveAsync();
        }

        public async Task<Image> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Image> GetMainImageForVehicle(int vehicleId)
        {
            return await _repo.GetMainImageForVehicle(vehicleId);
        }

        public async Task SetMainPhoto(int vehicleId, int photoId)
        {
            var photo = await GetMainImageForVehicle(vehicleId);
            photo.IsMain = false;

            var newMain = await GetByIdAsync(photoId);
            newMain.IsMain = true;

            await _repo.SaveAsync();
        }
    }
}
