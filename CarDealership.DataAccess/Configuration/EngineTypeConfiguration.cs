using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class EngineTypeConfiguration : IEntityTypeConfiguration<EngineTypeEntity>
    {
        public void Configure(EntityTypeBuilder<EngineTypeEntity> builder)
        {
            builder.HasKey(et => et.Id);

            builder.HasMany(et => et.Engines)
                .WithOne(e => e.EngineType)
                .HasForeignKey(e => e.EngineTypeId);
        }
    }
}
