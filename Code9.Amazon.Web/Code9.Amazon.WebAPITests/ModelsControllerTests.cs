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
    public class ModelsControllerTests
    {

        private readonly Mock<IModelService> _mockService;
        private readonly ModelsController _controller;

        public ModelsControllerTests()
        {
            _mockService = new Mock<IModelService>();

            var myProfile = new MappingProfile();
            var config = new MapperConfiguration(o => o.AddProfile(myProfile));
            var mapper = new Mapper(config);
            _controller = new ModelsController(_mockService.Object, mapper);
        }

        #region GetModelsTest

        [Fact]
        public async Task GetModels_ModelsFound_OkResult()
        {

            var models = CreateModels();

            _mockService.Setup(x => x.GetAllAsync()).ReturnsAsync(models);

            var okResult = await _controller.GetModels() as ObjectResult;
            var result = okResult.Value as List<KeyValuePairDto>;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(models.Count(), result.Count());
        }


        #endregion

        #region GetModelsByIdTest

        [Fact]
        public async Task GetModelById_ModelFoundAndReturned_OkResult()
        {
            var model = CreateModel();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(model);

            var okResponse = await _controller.GetModel(id) as ObjectResult;
            var result = okResponse.Value as KeyValuePairDto;

            Assert.IsType<OkObjectResult>(okResponse);
            Assert.Equal(model.Id, result.Id);
        }

        [Fact]
        public async Task GetModelById_ModelNotFound_NotFoundresult()
        {
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(() => null);

            var notFoundResult = await _controller.GetModel(id);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        #endregion



        #region GetModelsByModelId

        [Fact]
        public async Task GetModelsByMakeId_ModelsFound_OkResult()
        {
            var models = CreateModels();

            var id = 1;
            _mockService.Setup(x => x.GetByMakeIdAsync(id)).ReturnsAsync(models);

            var okResult = await _controller.GetModelsByMakeId(id) as ObjectResult;
            var result = okResult.Value as List<KeyValuePairDto>;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(models.Count(), result.Count());
        }


        #endregion

        #region CreateModelsTests

        [Fact]
        public async Task CreateMake_ModelStateValid_ReturnCreatedAtActionResult()
        {
            var modelDto = CreateModelToSaveDto();

            var createdResponse = await _controller.CreateModel(modelDto) as ObjectResult;
            var result = createdResponse.Value as KeyValuePairDto;

            Assert.IsType<KeyValuePairDto>(result);
            Assert.IsType<CreatedAtActionResult>(createdResponse);
            Assert.Equal(modelDto.Name, result.Name);
        }

        [Fact]
        public async Task CreateMake_InvalidModelState_ReturnBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");

            var modelDto = new ModelToSaveDto() { Name = "" };

            var result = await _controller.CreateModel(modelDto);

            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region UpdateModelsTests

        [Fact]
        public async Task UpdateMake_ModelStateValis_OkResult()
        {
            var modelToSave = CreateModelToSaveDto();
            var model = CreateModel();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(model);
            _mockService.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            var okResult = await _controller.UpdateModel(modelToSave, id);

            Assert.IsType<OkResult>(okResult);

        }

        [Fact]
        public async Task UpdateMake_InvalidModelState_ReturnBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");

            int id = 1;
            var modelDto = new ModelToSaveDto() { Name = "" };

            var badRequestResult = await _controller.UpdateModel(modelDto, id);

            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public async Task UpdateMake_ModelStateValid_ReturnNotFound()
        {
            var modelToSaveDto = CreateModelToSaveDto();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(() => null);

            var notFoundResult = await _controller.UpdateModel(modelToSaveDto, id);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        #endregion

        #region DeleteModelsTests

        [Fact]
        public async Task DeleteMake_MakeFoundAndDeleted()
        {
            var model = CreateModel();
            var id = 1;
            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(model);

            var okResult = await _controller.DeleteModel(id) as ObjectResult;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(id, okResult.Value);

        }

        [Fact]
        public async Task DeleteMake_MakeNotFound()
        {
            var id = 1;
            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(() => null);

            var result = await _controller.DeleteModel(id);

            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        private Model CreateModel()
        {
            return new Model()
            {
                Id = 1,
                Name = "Test Model",
                MakeId = 2
            };
        }

        private List<Model> CreateModels()
        {
            return new List<Model>()
            {
                new Model()
                {
                    Id = 1,
                    Name = "Test Model 1",
                    MakeId = 2
                },
                
                new Model()
                {
                    Id = 2,
                    Name = "Test Model 2",
                    MakeId = 5
                },

            };
        }

        private ModelToSaveDto CreateModelToSaveDto()
        {
            return new ModelToSaveDto()
            {
                Name = "Test Model",
                MakeId = 4
            };
        }

    }
}
