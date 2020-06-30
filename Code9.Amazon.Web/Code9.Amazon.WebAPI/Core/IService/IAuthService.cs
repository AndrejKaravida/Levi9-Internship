using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IService
{
    public interface IAuthService
    {
        Task Register(User user, string password);
        Task<User> Login(UserForLoginDto userForLoginDto);
        Task<bool> UserExists(string username);
        Task<string> GenerateJwtToken(User user);
    }
}
