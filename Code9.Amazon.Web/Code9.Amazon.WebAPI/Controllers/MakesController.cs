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
    public class MakesController : ControllerBase
    {
        private readonly IService<Make> _service;
        private readonly IMapper _mapper;

        public MakesController(IService<Make> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMakes()
        {
            var makes = await _service.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<MakeDto>>(makes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMake(int id)
        {
            var make = await _service.GetByIdAsync(id);
            if (make == null)
                return NotFound();
            return Ok(_mapper.Map<MakeDto>(make));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMake([FromBody] SingleValueToSaveDto makeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var make = _mapper.Map<Make>(makeDto);

            await _service.AddAsync(make);

            var result = _mapper.Map<MakeDto>(make);

            return CreatedAtAction(nameof(GetMake), new { id = result.Id }, result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMake([FromBody] SingleValueToSaveDto makeForEdit, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var makeInDb = await _service.GetByIdAsync(id);

            if (makeInDb == null)
                return NotFound();

             _mapper.Map<SingleValueToSaveDto, Make>(makeForEdit, makeInDb);

            if(await _service.SaveAsync())
                return Ok();

            return BadRequest("Make could not be saved");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMake(int id)
        {
            var make = await _service.GetByIdAsync(id);
            if (make == null)
                return NotFound();

            await _service.DeleteAsync(make);

            return Ok(id);

        }

    }
}