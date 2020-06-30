using AutoMapper;
using Code9.Amazon.WebAPI.Controllers;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Code9.Amazon.WebAPI.Dto;
using Code9.Amazon.WebAPI.Mapping;
using FluentAssertions;

namespace Code9.Amazon.WebAPITests
{
    public class FeaturesControllerTests
    {
        private readonly Mock<IService<Feature>> _mockService;
        private readonly FeaturesController _controller;
        public FeaturesControllerTests()
        {
            _mockService = new Mock<IService<Feature>>();

            var myProfile = new MappingProfile();
            var config = new MapperConfiguration(o => o.AddProfile(myProfile));
            var mapper = new Mapper(config);
            _controller = new FeaturesController(_mockService.Object, mapper);
        }

        #region GetFeaturesTests

        [Fact]
        public async Task GetFeatures_FeaturesFound_OkResult()
        {

            var features = CreateFeatures();

            _mockService.Setup(x => x.GetAllAsync()).ReturnsAsync(features);

            var okResult = await _controller.GetFeatures() as ObjectResult;
            var result = okResult.Value as List<KeyValuePairDto>;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(features.Count(), result.Count());
        }

        #endregion

        #region GetFeatureByIdTests

        [Fact]
        public async Task GetFeatureById_FeatureFoundAndReturned_OkResult()
        {
            var feature = CreateFeature();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(feature);

            var okResponse = await _controller.GetFeature(1) as ObjectResult;
            var result = okResponse.Value as KeyValuePairDto;

            Assert.IsType<OkObjectResult>(okResponse);
            Assert.Equal(feature.Id, result.Id);
        }

        [Fact]
        public async Task GetFeatureById_FeatureNotFound_NotFoundResult()
        {
            var feature = CreateFeature();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(() => null);

            var notFoundResult = await _controller.GetFeature(1);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }
        #endregion

        #region CreateFeatureTests
        [Fact]
        public async Task CreateFeature_ModelStateValid_ReturnCreatedAtActionResult()
        {
            var featureDto = CreateFeatureToSaveDto();

            var createdResponse = await _controller.CreateFeature(featureDto) as ObjectResult;
            var result = createdResponse.Value as KeyValuePairDto;

            Assert.IsType<KeyValuePairDto>(result);
            Assert.IsType<CreatedAtActionResult>(createdResponse);
            Assert.Equal(featureDto.Name, result.Name);
        }

        [Fact]
        public async Task CreateFeature_InvalidModelState_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");

            var featureDto = new SingleValueToSaveDto() { Name = "" };

            var result = await _controller.CreateFeature(featureDto);

            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region UpdateFeaturesTests

        [Fact]
        public async Task UpdateFeature_ModelStateValis_OkResult()
        {
            var featureToSaveDto = CreateFeatureToSaveDto();
            var feature = CreateFeature();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(feature);
            _mockService.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            var okResult = await _controller.UpdateFeature(featureToSaveDto, id);

            Assert.IsType<OkResult>(okResult);

        }

        [Fact]
        public async Task UpdateFeature_InvalidModelState_ReturnBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");

            int id = 1;
            var featureDto = new SingleValueToSaveDto() { Name = "" };

            var badRequestResult = await _controller.UpdateFeature(featureDto, id);

            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public async Task UpdateFeature_ModelStateValid_ReturnNotFound()
        {
            var featureToSaveDto = CreateFeatureToSaveDto();
            var id = 1;

            _mockService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(() => null);

            var notFoundResult = await _controller.UpdateFeature(featureToSaveDto, id);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        #endregion

        #region DeleteFeatureTests
        [Fact]
        public async Task DeleteFeature_FeatureFoundAndDeleted()
        {
            var feature = CreateFeature();
            var id = 1;
            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(feature);

            var okResult = await _controller.DeleteFeature(1) as ObjectResult;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(id, okResult.Value);
            
        }

        [Fact]
        public async Task DeleteFeature_FeatureNotFound()
        {
            var id = 1;
            _mockService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(() => null);

            var result = await _controller.DeleteFeature(id);

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion





        private Feature CreateFeature()
        {
            return new Feature()
            {
                Id = 1,
                Name = "Test Feature 1"
            };
        }

        private List<Feature> CreateFeatures()
        {
            return new List<Feature>()
            {
                new Feature(){Id = 1, Name = "Test Feature 1"},
                new Feature(){Id = 2, Name = "Test Feature 2"}
            };
        }

        private SingleValueToSaveDto CreateFeatureToSaveDto()
        {
            return new SingleValueToSaveDto()
            {
                Name = "Test Feature 1"
            };
        }
    }
}
