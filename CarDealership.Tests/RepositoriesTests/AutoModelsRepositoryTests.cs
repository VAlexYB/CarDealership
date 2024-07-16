using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using CarDealership.DataAccess.Repositories;
using CarDealership.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace CarDealership.Tests.RepositoriesTests
{
    public class AutoModelsRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<CarDealershipDbContext> _dbContextOptions;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly AutoModelsRepository _repository;
        private readonly Mock<IEntityModelFactory<AutoModel, AutoModelEntity>> _mockFactory;

        public AutoModelsRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CarDealershipDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
                .Options;

            _mockCache = new Mock<IDistributedCache>();
            _mockFactory = new Mock<IEntityModelFactory<AutoModel, AutoModelEntity>>();
            _repository = new AutoModelsRepository(new CarDealershipDbContext(_dbContextOptions), _mockFactory.Object, _mockCache.Object);
        }

        public void Dispose()
        {
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task InsertAsync_AddsNewAutoModel()
        {
            // Arrange
            var autoModel = AutoModel.Create(Guid.NewGuid(), "model1", 1000, Guid.NewGuid()).Value;

            _mockFactory.Setup(f => f.CreateEntity(autoModel))
                .Returns(new AutoModelEntity { Id = autoModel.Id, BrandId = autoModel.BrandId, Name = autoModel.Name, Price = autoModel.Price });

            // Act
            var result = await _repository.InsertAsync(autoModel);

            // Assert
            Assert.Equal(autoModel.Id, result);
        }

        [Fact]
        public async Task DeleteAsync_SoftDeletesAutoModel()
        {
            // Arrange
            var autoModelId = Guid.NewGuid();
            var autoModelEntity = new AutoModelEntity { Id = autoModelId, BrandId = Guid.NewGuid(), Name = "test", IsDeleted = false };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.AutoModels.Add(autoModelEntity);
                await context.SaveChangesAsync();
            }

            // Act
            await _repository.DeleteAsync(autoModelId);

            // Assert
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                var deletedEntity = await context.AutoModels.FindAsync(autoModelId);
                Assert.True(deletedEntity.IsDeleted);
            }
        }

        [Fact]
        public async Task GetFilteredAsync_ReturnsFilteredAutoModels()
        {
            // Arrange
            var brandId = Guid.NewGuid();
            var autoModels = new List<AutoModelEntity>
            {
                new AutoModelEntity { Id = Guid.NewGuid(), BrandId = brandId, Name = "Model A", IsDeleted = false },
                new AutoModelEntity { Id = Guid.NewGuid(), BrandId = brandId, Name = "Model B", IsDeleted = false },
                new AutoModelEntity { Id = Guid.NewGuid(), BrandId = Guid.NewGuid(), Name = "Model C", IsDeleted = false }
            };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.AutoModels.AddRange(autoModels);
                await context.SaveChangesAsync();
            }

            var filter = new AutoModelsFilter { BrandId = brandId };

            // Настройка мока для фабрики
            _mockFactory.Setup(f => f.CreateModel(It.IsAny<AutoModelEntity>()))
                .Returns((AutoModelEntity entity) => 
                AutoModel.Create(
                    entity.Id,
                    entity.Name,
                    entity.Price,
                    entity.BrandId
                ).Value);

            // Act
            var result = await _repository.GetFilteredAsync(filter);

            // Assert
            Assert.Equal(2, result.Count); // Ожидаем 2 модели с совпадающим BrandId
            Assert.All(result, model =>
            {
                Assert.NotNull(model); // Проверка, что объект не null
                Assert.Equal(brandId, model.BrandId); // Проверка, что все модели имеют правильный BrandId
                Assert.False(string.IsNullOrEmpty(model.Name)); // Проверка, что имя не null или пустое
            });
        }
    }

}
