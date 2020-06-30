using Code9.Amazon.WebAPI.Core.IRepository;
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
    public class MakeServiceTests
    {
        private Mock<IRepository<Make>> _mockRepository;
        private Service<Make> _service;

        public MakeServiceTests()
        {
            _mockRepository = new Mock<IRepository<Make>>();
            _service = new Service<Make>(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllMakes_MakesReturned()
        {
            var makes = CreateMakes();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(makes);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(makes.Count, result.ToList().Count);
        }

        [Fact]
        public async Task GetMake_MakeReturned()
        {
            var makes = CreateMake();
            var id = 1;

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(makes);

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(makes.Id, result.Id);
            Assert.Equal(makes.Name, result.Name);
        }

        [Fact]
        public async Task AddMake_AddAsyncCalledOnce()
        {
            var make = CreateMake();

            await _service.AddAsync(make);

            _mockRepository.Verify(x => x.Add(make), Times.Once());
        }

        [Fact]
        public async Task DeleteMake_DeleteAsyncCalledOnce()
        {
            var make = CreateMake();
            _mockRepository.Setup(r => r.Delete(It.IsAny<Make>()));

            await _service.DeleteAsync(make);

            _mockRepository.Verify(x => x.Delete(make), Times.Once);
        }



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
