using AutoMapper;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using Code9.Amazon.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVehicleService _vehicleService;
        private readonly IImageService _imageService;

        public VehiclesController(IMapper mapper, IVehicleService vehicleService, IImageService imageService)
        {
            _mapper = mapper;
            _vehicleService = vehicleService;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult GetVehicles([FromQuery]VehicleParams vehicleParams)
        {
            var vehicles =  _vehicleService.GetPagedDetailed(vehicleParams);

            var vehiclesToReturn = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);

            Response.AddPagination(vehicles.CurrentPage, vehicles.PageSize,
            vehicles.TotalCount, vehicles.TotalPages);

            return Ok(vehiclesToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            return Ok(_mapper.Map<VehicleDto>(vehicle));
        }

        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetAdminIdForVehicle(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle != null)
            {
                return Ok(vehicle.UserId);
            }

            return Ok(-1);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleToSaveDto vehicleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var vehicle = _mapper.Map<Vehicle>(vehicleDto);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            vehicle.UserId = userId;
            vehicle.LastUpdated = DateTime.Now;

            string startingPath = Startup.Configuration.GetSection("DefaultImageLocation").Value;

            await _vehicleService.AddAsync(vehicle);

            vehicle = await _vehicleService.GetByIdAsync(vehicle.Id);

            Image image = new Image()
            {
                IsMain = true,
                FileName = $"{startingPath}vehicle-placeholder.png",
                VehicleId = vehicle.Id
            };

            await _imageService.AddAsync(image);

            var result = _mapper.Map<VehicleDto>(vehicle);

            return CreatedAtAction(nameof(GetVehicleById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMake([FromBody] VehicleToSaveDto vehicleForEdit, int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!ModelState.IsValid)
                return BadRequest();

            var vehicleInDb = await _vehicleService.GetByIdAsync(id);

            if (vehicleInDb == null)
                return NotFound();

            if (userId != vehicleInDb.UserId)
                return Unauthorized();

            _mapper.Map<VehicleToSaveDto, Vehicle>(vehicleForEdit, vehicleInDb);
            vehicleInDb.LastUpdated = DateTime.Now;

            if(await _vehicleService.SaveAsync())
                return Ok(id);

            return BadRequest("Vehicle could not be saved");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            if (userId != vehicle.UserId)
                return Unauthorized();

            await _vehicleService.DeleteAsync(vehicle);

            return Ok(id);

        }
    }
}
