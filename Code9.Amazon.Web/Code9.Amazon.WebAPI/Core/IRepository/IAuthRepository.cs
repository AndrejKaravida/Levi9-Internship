using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.IRepository.Core
{
    public interface IAuthRepository
    {
        Task Register(User user, string password);
        Task<User> Login(UserForLoginDto userForLoginDto);
        Task<bool> UserExists(string username);
        Task<string> GenerateJwtToken(User user);
    }
}
