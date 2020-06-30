using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.IRepository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        public  AuthService(IAuthRepository repo)
        {
            _repo = repo;
        }
        public async Task<string> GenerateJwtToken(User user)
        {
            return await _repo.GenerateJwtToken(user);
        }

        public async Task<User> Login(UserForLoginDto userForLoginDto)
        {
            return await _repo.Login(userForLoginDto);
        }

        public async Task Register(User user, string password)
        {
            await _repo.Register(user, password);
        }

        public Task<bool> UserExists(string username)
        {
            return _repo.UserExists(username);
        }
    }
}
