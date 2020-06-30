using System.Threading.Tasks;
using AutoMapper;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Code9.Amazon.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IImageHandler _imageHandler;
        private readonly IImageService _imageService;
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public ImagesController(IImageHandler imageHandler, IVehicleService vehicleService, IMapper mapper, IImageService imageService)
        {
            _imageService = imageService;
            _imageHandler = imageHandler;
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var image = await _imageService.GetByIdAsync(id);

            return Ok(image);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            
            if (vehicle == null)
                return NotFound();

            if (file == null)
                return BadRequest("Please choose an image!");

            var image_location = await _imageHandler.UploadImage(file);
            var objectResult = image_location as ObjectResult;
            var value = objectResult.Value;

            string startingPath = Startup.Configuration.GetSection("DefaultImageLocation").Value;

            Image image = new Image()
            {
                FileName = $"{startingPath}{value}",
                VehicleId = id
            };

            await _imageService.AddAsync(image);

            return CreatedAtRoute("GetPhoto", new { id = image.Id }, image);
        }

        [HttpPost("setmain/{vehicleId}/{photoId}")]
        public async Task<IActionResult> SetMain(int vehicleId, int photoId)
        {
            await _imageService.SetMainPhoto(vehicleId, photoId);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _imageService.GetByIdAsync(id);
            await _imageService.DeleteAsync(photo);

            return Ok();
        }

    }
}