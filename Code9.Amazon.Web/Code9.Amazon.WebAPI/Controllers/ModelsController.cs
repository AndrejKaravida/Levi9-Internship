using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Code9.Amazon.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModelsController : ControllerBase
    {
        private readonly IModelService _service;
        private readonly IMapper _mapper;

        public ModelsController(IModelService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetModels()
        {
            var models = await _service.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<KeyValuePairDto>>(models));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModel(int id)
        {
            var model = await _service.GetByIdAsync(id);

            if (model == null)
                return NotFound();

            return Ok(_mapper.Map<KeyValuePairDto>(model));
        }

        [HttpGet("make/{id}")]
        public async Task<IActionResult> GetModelsByMakeId(int id)
        {
            var models = await _service.GetByMakeIdAsync(id);

            return Ok(_mapper.Map<IEnumerable<KeyValuePairDto>>(models));
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel([FromBody] ModelToSaveDto modelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var model = _mapper.Map<Model>(modelDto);

            await _service.AddAsync(model);

            var result = _mapper.Map<KeyValuePairDto>(model);

            return  CreatedAtAction(nameof(GetModel), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModel([FromBody] ModelToSaveDto modelForEdit, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var modelInDb = await _service.GetByIdAsync(id);

            if (modelInDb == null)
                return NotFound();

            _mapper.Map<ModelToSaveDto, Model>(modelForEdit, modelInDb);
            
            if (await _service.SaveAsync())
                return Ok();

            return BadRequest("Model could not be saved");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var make = await _service.GetByIdAsync(id);
            if (make == null)
                return NotFound();

            await _service.DeleteAsync(make);

            return Ok(id);
        }

    }
}