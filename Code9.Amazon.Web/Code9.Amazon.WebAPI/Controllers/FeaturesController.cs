using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Code9.Amazon.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Code9.Amazon.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeaturesController : ControllerBase
    {
        private readonly IService<Feature> _service;
        private readonly IMapper _mapper;

        public FeaturesController(IService<Feature> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetFeatures()
        {
            var features = await _service.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<KeyValuePairDto>>(features));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeature(int id)
        {
            var feature = await _service.GetByIdAsync(id);
            if (feature == null)
                return NotFound();
            return Ok(_mapper.Map<KeyValuePairDto>(feature));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeature([FromBody] SingleValueToSaveDto featureDto )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var feature = _mapper.Map<Feature>(featureDto);

            await _service.AddAsync(feature);

             var result = _mapper.Map<KeyValuePairDto>(feature);

            return CreatedAtAction("GetFeature", new { id = result.Id }, result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeature([FromBody] SingleValueToSaveDto featureForEdit, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var makeInDb = await _service.GetByIdAsync(id);

            if (makeInDb == null)
                return NotFound();

            _mapper.Map<SingleValueToSaveDto, Feature>(featureForEdit, makeInDb);

            if (await _service.SaveAsync())
                return Ok();

            return BadRequest("Feature could not be saved");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var make = await _service.GetByIdAsync(id);
            if (make == null)
                return NotFound();

            await _service.DeleteAsync(make);

            return Ok(id);

        }

    }
}