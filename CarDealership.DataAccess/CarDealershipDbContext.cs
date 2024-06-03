using CarDealership.DataAccess.Configuration;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.DataAccess
{
    public class CarDealershipDbContext : DbContext
    {
        public CarDealershipDbContext(DbContextOptions<CarDealershipDbContext> options) : base(options)
        {
        }

        public DbSet<AutoModelEntity> AutoModels { get; set; }
        public DbSet<AutoConfigurationEntity> AutoConfigurations { get; set; }
        public DbSet<BodyTypeEntity> BodyTypes { get; set; }
        public DbSet<BrandEntity> Brands { get; set; }
        public DbSet<CarEntity> Cars { get; set; }
        public DbSet<ColorEntity> Colors { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<DriveTypeEntity> DriveTypes { get; set; }
        public DbSet<EngineEntity> Engines { get; set; }
        public DbSet<EngineTypeEntity> EngineTypes { get; set; }
        public DbSet<EquipmentEntity> Equipments { get; set; }
        public DbSet<EquipmentFeatureEntity> EquipmentFeatures { get; set; }
        public DbSet<FeatureEntity> Features { get; set; }
        public DbSet<TransmissionTypeEntity> TransmissionTypes { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<DealEntity> Deals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new AutoModelConfiguration())
                .ApplyConfiguration(new AutoConfConfiguration())
                .ApplyConfiguration(new BodyTypeConfiguration())
                .ApplyConfiguration(new BrandConfiguration())
                .ApplyConfiguration(new CarConfiguration())
                .ApplyConfiguration(new ColorConfiguration())
                .ApplyConfiguration(new CountryConfiguration())
                .ApplyConfiguration(new DriveTypeConfiguration())
                .ApplyConfiguration(new EngineConfiguration())
                .ApplyConfiguration(new EngineTypeConfiguration())
                .ApplyConfiguration(new EquipmentConfiguration())
                .ApplyConfiguration(new EquipmentFeatureConfiguration())
                .ApplyConfiguration(new FeatureConfiguration())
                .ApplyConfiguration(new TransmissionTypeConfiguration())
                .ApplyConfiguration(new UserConfiguration())
                .ApplyConfiguration(new OrderConfiguration())
                .ApplyConfiguration(new DealConfiguration())
                .ApplyConfiguration(new RoleConfiguration());
            

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
