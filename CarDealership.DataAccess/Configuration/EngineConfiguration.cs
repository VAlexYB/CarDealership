using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class EngineConfiguration : IEntityTypeConfiguration<EngineEntity>
    {
        public void Configure(EntityTypeBuilder<EngineEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.EngineType)
                .WithMany(et => et.Engines)
                .HasForeignKey(e => e.EngineTypeId);

            builder.HasOne(e => e.TransmissionType)
                .WithMany(tt => tt.Engines)
                .HasForeignKey(e => e.TransmissionTypeId);

            builder.HasMany(e => e.Configurations)
                .WithOne(c => c.Engine)
                .HasForeignKey(c => c.EngineId);
        }
    }
}
