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
    public class AutoConfigsRepositoryTests : IDisposable
    {
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly DbContextOptions<CarDealershipDbContext> _dbContextOptions;
        private readonly AutoConfigsRepository _repository;
        private readonly Mock<IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity>> _mockFactory;

        public AutoConfigsRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CarDealershipDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _mockCache = new Mock<IDistributedCache>();
            _mockFactory = new Mock<IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity>>();
            _repository = new AutoConfigsRepository(new CarDealershipDbContext(_dbContextOptions), _mockFactory.Object, _mockCache.Object);
        }

        public void Dispose()
        {
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAutoConfiguration_WhenExists()
        {
            // Arrange
            var autoConfigId = Guid.NewGuid();
            var existingEntity = new AutoConfigurationEntity
            {
                Id = autoConfigId,
                AutoModelId = Guid.NewGuid(),
                BodyTypeId = Guid.NewGuid(),
                DriveTypeId = Guid.NewGuid(),
                EngineId = Guid.NewGuid(),
                ColorId = Guid.NewGuid(),
                EquipmentId = Guid.NewGuid(),
                IsDeleted = false
            };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.AutoConfigurations.Add(existingEntity);
                await context.SaveChangesAsync();
            }

            var updatedConfig = AutoConfiguration.Create(
                autoConfigId,
                1000,
                existingEntity.AutoModelId,
                existingEntity.BodyTypeId,
                Guid.NewGuid(), // Change one property for testing
                existingEntity.EngineId,
                existingEntity.ColorId,
                existingEntity.EquipmentId
            ).Value;

            _mockFactory.Setup(f => f.CreateEntity(updatedConfig))
                .Returns(new AutoConfigurationEntity
                {
                    Id = updatedConfig.Id,
                    AutoModelId = updatedConfig.AutoModelId,
                    BodyTypeId = updatedConfig.BodyTypeId,
                    DriveTypeId = updatedConfig.DriveTypeId,
                    EngineId = updatedConfig.EngineId,
                    ColorId = updatedConfig.ColorId,
                    EquipmentId = updatedConfig.EquipmentId
                });

            // Act
            var result = await _repository.UpdateAsync(updatedConfig);

            // Assert
            Assert.Equal(autoConfigId, result);
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                var updatedEntity = await context.AutoConfigurations.FindAsync(autoConfigId);
                Assert.Equal(updatedConfig.DriveTypeId, updatedEntity.DriveTypeId);
            }
        }

        [Fact]
        public async Task GetFilteredAsync_ReturnsFilteredAutoConfigurations()
        {
            // Arrange
            var autoConfig1 = new AutoConfigurationEntity
            { 
                Id = Guid.NewGuid(),
                AutoModelId = Guid.NewGuid(),
                BodyTypeId = Guid.NewGuid(),
                DriveTypeId = Guid.NewGuid(),
                EngineId = Guid.NewGuid(),
                ColorId = Guid.NewGuid(),
                EquipmentId = Guid.NewGuid(),
                IsDeleted = false 
            };
            var autoConfig2 = new AutoConfigurationEntity {
                Id = Guid.NewGuid(),
                AutoModelId = autoConfig1.AutoModelId,
                BodyTypeId = Guid.NewGuid(),
                DriveTypeId = Guid.NewGuid(),
                EngineId = Guid.NewGuid(),
                ColorId = Guid.NewGuid(),
                EquipmentId = Guid.NewGuid(),
                IsDeleted = false 
            };
            var autoConfig3 = new AutoConfigurationEntity
            {
                Id = Guid.NewGuid(),
                AutoModelId = Guid.NewGuid(),
                BodyTypeId = Guid.NewGuid(),
                DriveTypeId = Guid.NewGuid(),
                EngineId = Guid.NewGuid(),
                ColorId = Guid.NewGuid(),
                EquipmentId = Guid.NewGuid(),
                IsDeleted = false
            };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.AutoConfigurations.AddRange(autoConfig1, autoConfig2, autoConfig3);
                await context.SaveChangesAsync();
            }

            var filter = new ConfigurationsFilter { AutoModelId = autoConfig1.AutoModelId };

            _mockFactory.Setup(f => f.CreateModel(It.IsAny<AutoConfigurationEntity>()))
                .Returns((AutoConfigurationEntity entity) => 
                AutoConfiguration.Create(
                    entity.Id,
                    1000,
                    entity.AutoModelId,
                    entity.BodyTypeId,
                    entity.DriveTypeId,
                    entity.EngineId,
                    entity.ColorId,
                    entity.EquipmentId
                ).Value);

            // Act
            var result = await _repository.GetFilteredAsync(filter);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAutoConfigurations()
        {
            // Arrange
            var autoConfig1 = new AutoConfigurationEntity
            {
                Id = Guid.NewGuid(),
                AutoModelId = Guid.NewGuid(),
                BodyTypeId = Guid.NewGuid(),
                DriveTypeId = Guid.NewGuid(),
                EngineId = Guid.NewGuid(),
                ColorId = Guid.NewGuid(),
                EquipmentId = Guid.NewGuid(),
                IsDeleted = false
            };
            var autoConfig2 = new AutoConfigurationEntity
            {
                Id = Guid.NewGuid(),
                AutoModelId = Guid.NewGuid(),
                BodyTypeId = Guid.NewGuid(),
                DriveTypeId = Guid.NewGuid(),
                EngineId = Guid.NewGuid(),
                ColorId = Guid.NewGuid(),
                EquipmentId = Guid.NewGuid(),
                IsDeleted = false
            };
            var autoConfig3 = new AutoConfigurationEntity
            {
                Id = Guid.NewGuid(),
                AutoModelId = Guid.NewGuid(),
                BodyTypeId = Guid.NewGuid(),
                DriveTypeId = Guid.NewGuid(),
                EngineId = Guid.NewGuid(),
                ColorId = Guid.NewGuid(),
                EquipmentId = Guid.NewGuid(),
                IsDeleted = true // удаленная конфигурация
            };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.AutoConfigurations.AddRange(autoConfig1, autoConfig2, autoConfig3);
                await context.SaveChangesAsync();
            }

            _mockFactory.Setup(f => f.CreateModel(It.IsAny<AutoConfigurationEntity>()))
                .Returns((AutoConfigurationEntity entity) => AutoConfiguration.Create(entity.Id, 1000, entity.AutoModelId, entity.BodyTypeId, entity.DriveTypeId, entity.EngineId, entity.ColorId, entity.EquipmentId).Value);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count); // 2 должны вернуться, т.к.  1 из 3 удалена
        }


        [Fact]
        public async Task InsertAsync_AddsNewAutoConfiguration()
        {
            // Arrange
            var newConfig = AutoConfiguration.Create(Guid.NewGuid(), 1000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value;
            _mockFactory.Setup(f => f.CreateEntity(newConfig))
                .Returns(new AutoConfigurationEntity { Id = newConfig.Id, AutoModelId = newConfig.AutoModelId });

            // Act
            var result = await _repository.InsertAsync(newConfig);

            // Assert
            Assert.Equal(newConfig.Id, result);
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                var addedEntity = await context.AutoConfigurations.FindAsync(newConfig.Id);
                Assert.NotNull(addedEntity);
            }
        }

        [Fact]
        public async Task DeleteAsync_SoftDeletesAutoConfiguration()
        {
            // Arrange
            var autoConfigId = Guid.NewGuid();
            var autoConfigEntity = new AutoConfigurationEntity { Id = autoConfigId, AutoModelId = Guid.NewGuid(), IsDeleted = false };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.AutoConfigurations.Add(autoConfigEntity);
                await context.SaveChangesAsync();
            }

            // Act
            await _repository.DeleteAsync(autoConfigId);

            // Assert
            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                var deletedEntity = await context.AutoConfigurations.FindAsync(autoConfigId);
                Assert.True(deletedEntity.IsDeleted);
            }
        }

        [Fact]
        public async Task ExistsAsync_ReturnsTrue_WhenAutoConfigurationExists()
        {
            // Arrange
            var autoConfigId = Guid.NewGuid();
            var autoConfigEntity = new AutoConfigurationEntity { Id = autoConfigId, AutoModelId = Guid.NewGuid(), IsDeleted = false };

            using (var context = new CarDealershipDbContext(_dbContextOptions))
            {
                context.AutoConfigurations.Add(autoConfigEntity);
                await context.SaveChangesAsync();
            }

            // Act
            var exists = await _repository.ExistsAsync(autoConfigId);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalse_WhenAutoConfigurationDoesNotExist()
        {
            // Act
            var exists = await _repository.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.False(exists);
        }
    }
}
