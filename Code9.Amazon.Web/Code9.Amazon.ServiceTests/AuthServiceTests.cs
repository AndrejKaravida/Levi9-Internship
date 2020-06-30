using Code9.Amazon.WebAPI;
using Code9.Amazon.WebAPI.IRepository.Core;
using Code9.Amazon.WebAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Code9.Amazon.ServiceTests
{
    public class AuthServiceTests
    {
        private Mock<IAuthRepository> _mockRepository;
        private AuthService _service;

        public AuthServiceTests()
        {
            _mockRepository = new Mock<IAuthRepository>();
            _service = new AuthService(_mockRepository.Object);
        }

        [Fact]
        public async Task Login_LoginReturnedUser()
        {
            var userForLoginDto = CreateUserForLogin();
            var user = CreateUser();

            _mockRepository.Setup(x => x.Login(userForLoginDto)).ReturnsAsync(user);

            var result = await _service.Login(userForLoginDto);

            Assert.IsType<User>(result);
        }

        [Fact]
        public async Task Login_LoginResturnedNull()
        {
            var userForLoginDto = CreateUserForLogin();

            _mockRepository.Setup(x => x.Login(userForLoginDto)).ReturnsAsync(() => null);

            var result = await _service.Login(userForLoginDto);

            Assert.True(result == null);
        }

        [Fact]
        public async Task Register_RegisterCalledOnce()
        {
            var user = CreateUser();
            var password = "password123";

            await _service.Register(user, password);

            _mockRepository.Verify(x => x.Register(user, password), Times.Once);
        }

        [Fact]
        public async Task UserExists_ReturnsTrue()
        {
            var user = CreateUserForLogin();

            _mockRepository.Setup(x => x.UserExists(user.Username)).ReturnsAsync(true);

            var result = await _service.UserExists(user.Username);

            Assert.True(result == true);
        }

        [Fact]
        public async Task UserExists_ReturnsFalse()
        {
            var user = CreateUserForLogin();

            _mockRepository.Setup(x => x.UserExists(user.Username)).ReturnsAsync(false);

            var result = await _service.UserExists(user.Username);

            Assert.True(result == false);
        }

        [Fact]
        public async Task GenerateJwtToke_ReturnsToken()
        {
            var user = CreateUser();
            string token = "somerandomgeneratedtoken";

            _mockRepository.Setup(x => x.GenerateJwtToken(user)).ReturnsAsync(token);

            var result = await _service.GenerateJwtToken(user);

            Assert.IsType<string>(result);
            Assert.Equal(token, result);
        }


        private UserForLoginDto CreateUserForLogin()
        {
            return new UserForLoginDto()
            {
                Username = "johnny",
                Password = "password123"
            };
        }

        private User CreateUser()
        {
            return new User()
            {
                FirstName = "John",
                LastName = "Johansson"
            };
        }

    }
}
