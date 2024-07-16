using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using CarDealership.DataAccess.Repositories;
using CarDealership.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CarDealership.Tests.RepositoriesTests
{
    public class BodyTypesRepositoryTests : IDisposable
    {
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly DbContextOptions<CarDealershipDbContext> _dbContextOptions;
        private readonly BodyTypesRepository _repository;
        private readonly Mock<IEntityModelFactory<BodyType, BodyTypeEntity>> _mockFactory;

        public BodyTypesRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CarDealershipDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _mockCache = new Mock<IDistributedCache>();
            _mockFactory = new Mock<IEntityModelFactory<BodyType, BodyTypeEntity>>();
            var context = new CarDealershipDbContext(_dbContextOptions);
            _repository = new BodyTypesRepository(context, _mockFactory.Object, _mockCache.Object);
        }

        public void Dispose()
        {
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted(); 
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBodyTypes()
        {
            // Arrange
            var bodyTypes = new List<BodyTypeEntity>
        {
            new BodyTypeEntity { Id = Guid.NewGuid(), Value = "Sedan", IsDeleted = false },
            new BodyTypeEntity { Id = Guid.NewGuid(), Value = "SUV", IsDeleted = false }
        };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.BodyTypes.AddRange(bodyTypes);
                await context.SaveChangesAsync();
            }

            _mockFactory.Setup(f => f.CreateModel(It.IsAny<BodyTypeEntity>()))
                .Returns((BodyTypeEntity entity) => BodyType.Create(entity.Id, entity.Value, 1000).Value);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBodyType_WhenExists()
        {
            // Arrange
            var bodyTypeId = Guid.NewGuid();
            var bodyTypeEntity = new BodyTypeEntity { Id = bodyTypeId, Value = "Hatchback", IsDeleted = false };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.BodyTypes.Add(bodyTypeEntity);
                await context.SaveChangesAsync();
            }

            _mockFactory.Setup(f => f.CreateModel(It.IsAny<BodyTypeEntity>()))
                .Returns((BodyTypeEntity entity) => BodyType.Create(entity.Id, entity.Value, 2000).Value);

            // Act
            var result = await _repository.GetByIdAsync(bodyTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bodyTypeId, result.Id);
        }

        [Fact]
        public async Task InsertAsync_AddsNewBodyType()
        {
            // Arrange
            var bodyType = BodyType.Create(Guid.NewGuid(), "Coupe", 1000).Value;

            _mockFactory.Setup(f => f.CreateEntity(bodyType))
                .Returns(new BodyTypeEntity { Id = bodyType.Id, Value = bodyType.Value, IsDeleted = false });

            // Act
            var result = await _repository.InsertAsync(bodyType);

            // Assert
            Assert.Equal(bodyType.Id, result);

            // Verify the body type was added
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                Assert.Equal(1, await context.BodyTypes.CountAsync());
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingBodyType()
        {
            // Arrange
            var bodyTypeId = Guid.NewGuid();
            var bodyTypeEntity = new BodyTypeEntity { Id = bodyTypeId, Value = "Convertible", IsDeleted = false };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.BodyTypes.Add(bodyTypeEntity);
                await context.SaveChangesAsync();
            }

            var updatedBodyType = BodyType.Create(bodyTypeId, "Updated", 1000).Value;
            _mockFactory.Setup(f => f.CreateEntity(updatedBodyType))
                .Returns(new BodyTypeEntity { Id = bodyTypeId, Value = updatedBodyType.Value, IsDeleted = false });

            // Act
            await _repository.UpdateAsync(updatedBodyType);

            // Assert
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                var updatedEntity = await context.BodyTypes.FindAsync(bodyTypeId);
                Assert.Equal("Updated", updatedEntity.Value);
            }
        }

        [Fact]
        public async Task DeleteAsync_SoftDeletesBodyType()
        {
            // Arrange
            var bodyTypeId = Guid.NewGuid();
            var bodyTypeEntity = new BodyTypeEntity { Id = bodyTypeId, Value = "Minivan", IsDeleted = false };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.BodyTypes.Add(bodyTypeEntity);
                await context.SaveChangesAsync();
            }

            // Act
            await _repository.DeleteAsync(bodyTypeId);

            // Assert
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                var deletedEntity = await context.BodyTypes.FindAsync(bodyTypeId);
                Assert.True(deletedEntity.IsDeleted);
            }
        }

        [Fact]
        public async Task ExistsAsync_ReturnsTrue_WhenBodyTypeExists()
        {
            // Arrange
            var bodyTypeId = Guid.NewGuid();
            var bodyTypeEntity = new BodyTypeEntity { Id = bodyTypeId, Value = "Pickup", IsDeleted = false };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.BodyTypes.Add(bodyTypeEntity);
                await context.SaveChangesAsync();
            }

            // Act
            var exists = await _repository.ExistsAsync(bodyTypeId);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalse_WhenBodyTypeDoesNotExist()
        {
            // Act
            var exists = await _repository.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.False(exists);
        }
    }
}
