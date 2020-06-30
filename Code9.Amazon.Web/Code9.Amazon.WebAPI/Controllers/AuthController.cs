using System.Threading.Tasks;
using AutoMapper;
using Code9.Amazon.WebAPI.Core;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.IRepository.Core;
using Microsoft.AspNetCore.Mvc;

namespace Code9.Amazon.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IMapper _mapper;

        public AuthController(IAuthService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (await _service.UserExists(userForRegisterDto.Username.ToUpper()))
                return BadRequest("Username already exists");

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            await _service.Register(userToCreate, userForRegisterDto.Password);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            if (!await _service.UserExists(userForLoginDto.Username.ToUpper()))
                return NotFound("There is no user with that username");
            
            var appUser = await _service.Login(userForLoginDto);


            var userToReturn = _mapper.Map<UserToListDto>(appUser);
            
            if(appUser != null) 
            {
                return Ok(
                    new
                    {
                        token = await _service.GenerateJwtToken(appUser),
                        user = userToReturn
                    });
            }  

            return Unauthorized();
        }


    }
}
