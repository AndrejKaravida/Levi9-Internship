using AutoMapper;
using Code9.Amazon.WebAPI.Controllers;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using Code9.Amazon.WebAPI.Mapping;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Code9.Amazon.WebAPITests
{
    public class MakesControllerTests
    {
        private readonly Mock<IService<Make>> _mockService;
        private readonly MakesController _controller;

        public MakesControllerTests()
        {
            _mockService = new Mock<IService<Make>>();

            var myProfile = new MappingProfile();
            var config = new MapperConfiguration(o => o.AddProfile(myProfile));
            var mapper = new Mapper(config);
            _controller = new MakesController(_mockService.Object, mapper);
        }

        #region GetMakesTests
        [Fact]
        public async Task GetMakes_MakesFound_OkResult()
        {
            var makes = CreateMakes();

            _mockService.Setup(x => x.GetAllAsync()).ReturnsAsync(makes);

            var okResult = await _controller.GetMakes() as ObjectResult;
            var result = okResult.Value as List<MakeDto>;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(makes.Count(), result.Count());
        }


        #endregion

        #region GetMakeByIdTests
        
        [Fact]
        public async Task GetMakeById_MakeFoundAndReturned_OkResult()
        {
            var make = CreateMake();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(make);

            var okResponse = await _controller.GetMake(id) as ObjectResult;
            var result = okResponse.Value as MakeDto;

            Assert.IsType<OkObjectResult>(okResponse);
            Assert.Equal(make.Id, result.Id);
        }

        [Fact]
        public async Task GetMakeById_MakeNotFound_NotFoundresult()
        {
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(() => null);

            var notFoundResult = await _controller.GetMake(id);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }
        #endregion

        #region CreateMakeTests

        [Fact]
        public async Task CreateMake_ModelStateValid_ReturnCreatedAtActionResult()
        {
            var makeDto = CreateMakeToSaveDto();

            var createdResponse = await _controller.CreateMake(makeDto) as ObjectResult;
            var result = createdResponse.Value as MakeDto;

            Assert.IsType<MakeDto>(result);
            Assert.IsType<CreatedAtActionResult>(createdResponse);
            Assert.Equal(makeDto.Name, result.Name);
        }

        [Fact]
        public async Task CreateMake_InvalidModelState_ReturnBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");

            var makeDto = new SingleValueToSaveDto() { Name = "" };

            var result = await _controller.CreateMake(makeDto);

            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region UpdateMakeTests

        [Fact]
        public async Task UpdateMake_ModelStateValis_OkResult()
        {
            var makeDto = CreateMakeToSaveDto();
            var make = CreateMake();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(make);
            _mockService.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            var okResult = await _controller.UpdateMake
                (makeDto, id);

            Assert.IsType<OkResult>(okResult);

        }

        [Fact]
        public async Task UpdateMake_InvalidModelState_ReturnBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");

            int id = 1;
            var makeDto = new SingleValueToSaveDto() { Name = "" };

            var badRequestResult = await _controller.UpdateMake(makeDto, id);

            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public async Task UpdateMake_ModelStateValid_ReturnNotFound()
        {
            var makeToSaveDto = CreateMakeToSaveDto();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(() => null);

            var notFoundResult = await _controller.UpdateMake(makeToSaveDto, id);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        #endregion

        #region DeteleMakeTests

        [Fact]
        public async Task DeleteMake_MakeFoundAndDeleted()
        {
            var make = CreateMake();
            var id = 1;
            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(make);

            var okResult = await _controller.DeleteMake(id) as ObjectResult;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(id, okResult.Value);

        }

        [Fact]
        public async Task DeleteMake_MakeNotFound()
        {
            var id = 1;
            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(() => null);

            var result = await _controller.DeleteMake(id);

            Assert.IsType<NotFoundResult>(result);
        }

        #endregion


        private Make CreateMake()
        {
            return new Make()
            {
                Id = 1,
                Name = "Test Make",
                Models = new List<Model>()
                {
                    new Model(){Id = 1, Name = "Test1"},
                    new Model(){Id = 1, Name = "Test1"}
                }
            };
        }

        private SingleValueToSaveDto CreateMakeToSaveDto()
        {
            return new SingleValueToSaveDto()
            {
                Name = "Test Make"
            };
        }

        private List<Make> CreateMakes()
        {
            return new List<Make>()
            {
                new Make()
                {
                    Id = 1, 
                    Name = "Test Make 1",
                    Models = new List<Model>()
                    {
                        new Model(){Id = 1, Name = "Test1"},
                        new Model(){Id = 2, Name = "Test2"}
                    }
                },
                new Make()
                {
                    Id = 2,
                    Name = "Test Make 2",
                    Models = new List<Model>()
                    {
                        new Model(){Id = 3, Name = "Test3"},
                        new Model(){Id = 4, Name = "Test4"}
                    }
                }
            };
        }

    }

}
