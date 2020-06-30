using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
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
    public class FeatureServiceTests
    {
        private Mock<IRepository<Feature>> _mockRepository;
        private Service<Feature> _service;

        public FeatureServiceTests()
        {
            _mockRepository = new Mock<IRepository<Feature>>();
            _service = new Service<Feature>(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllFeatures_FeaturesReturned()
        {
            var features = CreateFeatures();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(features);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(features.Count, result.ToList().Count);
        }

        [Fact]
        public async Task GetFeature_FeatureReturned()
        {
            var feature = CreateFeature();
            var id = 1;

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(feature);

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(feature.Id, result.Id);
            Assert.Equal(feature.Name, result.Name);
        }


        [Fact]
        public async Task AddFeature_AddAsyncCalledOnce()
        {
            var feature = CreateFeature();

            await _service.AddAsync(feature);

            _mockRepository.Verify(x => x.Add(feature), Times.Once());
        }
        
        [Fact]
        public async Task DeleteFeature_DeleteAsyncCalledOnce()
        {
            var feature = CreateFeature();
            _mockRepository.Setup(r => r.Delete(It.IsAny<Feature>()));

            await _service.DeleteAsync(feature);

            _mockRepository.Verify(x => x.Delete(feature), Times.Once);
        }

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



    }
}
