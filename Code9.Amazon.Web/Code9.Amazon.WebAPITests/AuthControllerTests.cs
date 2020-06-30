using AutoMapper;
using Code9.Amazon.WebAPI;
using Code9.Amazon.WebAPI.Controllers;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Mapping;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Code9.Amazon.WebAPITests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockSerivce;
        private readonly AuthController _controller;
        public AuthControllerTests()
        {
            _mockSerivce = new Mock<IAuthService>();

            var myProfile = new MappingProfile();
            var config = new MapperConfiguration(o => o.AddProfile(myProfile));
            var mapper = new Mapper(config);
            _controller = new AuthController(_mockSerivce.Object, mapper);
        }

        #region UserRegisterTests

        [Fact]
        public async Task RegisterUser_ModelStateInvalid_BadRequestResult()
        {
            _controller.ModelState.AddModelError("Email", "Email is required");

            var userForRegister = CreateUserForRegister();

            var result = await _controller.Register(userForRegister);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task RegisterUser_ValidModelState_UsernameExists_BadRequestResult()
        {
            var userForRegister = CreateUserForRegister();

            _mockSerivce.Setup(x => x.UserExists(userForRegister.Username.ToUpper())).ReturnsAsync(true);

            var result = await _controller.Register(userForRegister);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RegisterUser_ValidModelState_OkResult()
        {
            var userForRegister = CreateUserForRegister();
            _mockSerivce.Setup(x => x.UserExists(userForRegister.Username.ToUpper())).ReturnsAsync(false);

            var result = await _controller.Register(userForRegister);

            Assert.IsType<OkResult>(result);
        }


        #endregion

        #region UserLoginTests

        [Fact]
        public async Task LoginUser_UserLoggedIn()
        {
            var userForLoginDto = CreateUserForLogin();
            var user = CreateUser();

            _mockSerivce.Setup(x => x.UserExists(userForLoginDto.Username.ToUpper())).ReturnsAsync(true);
            _mockSerivce.Setup(x => x.Login(userForLoginDto)).ReturnsAsync(user);

            var okResult = await _controller.Login(userForLoginDto);

            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task LoginUser_InvalidUsername()
        {
            var userForLoginDto = CreateUserForLogin();

            _mockSerivce.Setup(x => x.UserExists(userForLoginDto.Username.ToUpper())).ReturnsAsync(false);

            var notFoundResult = await _controller.Login(userForLoginDto);

            Assert.IsType<NotFoundObjectResult>(notFoundResult);

        }

        [Fact]
        public async Task LoginUser_InvalidCredentials()
        {
            var userForLoginDto = CreateUserForLogin();

            _mockSerivce.Setup(x => x.UserExists(userForLoginDto.Username.ToUpper())).ReturnsAsync(true);
            _mockSerivce.Setup(x => x.Login(userForLoginDto)).ReturnsAsync(() => null);

            var unauthorizedResult = await _controller.Login(userForLoginDto);

            Assert.IsType<UnauthorizedResult>(unauthorizedResult);

        }

        #endregion

        private UserForRegisterDto CreateUserForRegister()
        {
            return new UserForRegisterDto()
            {
                FirstName = "John",
                LastName = "Johansson",
                Username = "johnny",
                Password = "password123"
            };
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
