using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.IRepository.Core;
using Code9.Amazon.WebAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Code9.Amazon.ServiceTests
{
    public class ModelServiceTests
    {
        private Mock<IModelRepository> _mockRepository;
        private ModelService _service;


        public ModelServiceTests()
        {
            _mockRepository = new Mock<IModelRepository>();
            _service = new ModelService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllModels_ModelsReturned()
        {
            var models = CreateModels();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(models);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(models.Count, result.ToList().Count);
        }

        [Fact]
        public async Task GetModelsByMakeId_ModelsReturned()
        {
            var models = CreateModels();
            var id = 1;

            _mockRepository.Setup(x => x.GetByMakeIdAsync(id)).ReturnsAsync(models);

            var result = await _service.GetByMakeIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(models.Count, result.ToList().Count);
        }

        [Fact]
        public async Task GetModel_ModelReturned()
        {
            var model = CreateModel();
            var id = 1;

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(model);

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(model.Id, result.Id);
            Assert.Equal(model.Name, result.Name);
        }


        [Fact]
        public async Task AddModel_AddAsyncCalledOnce()
        {
            var model = CreateModel();

            await _service.AddAsync(model);

            _mockRepository.Verify(x => x.Add(model), Times.Once());
        }

        [Fact]
        public async Task DeleteModel_DeleteAsyncCalledOnce()
        {
            var model = CreateModel();
            _mockRepository.Setup(r => r.Delete(It.IsAny<Model>()));

            await _service.DeleteAsync(model);

            _mockRepository.Verify(x => x.Delete(model), Times.Once);
        }


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


    }
}
