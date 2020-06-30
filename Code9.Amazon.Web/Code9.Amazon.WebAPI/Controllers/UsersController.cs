using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Code9.Amazon.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UsersController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsers([FromQuery]UserParams userParams)
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            userParams.UserId = id;

            var users = _service.GetPaged(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserToListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        { 
            var user = await _service.GetByIdAsync(id);

            var userToReturn = _mapper.Map<UserToListDto>(user);

            return Ok(userToReturn);
        }
    }
}