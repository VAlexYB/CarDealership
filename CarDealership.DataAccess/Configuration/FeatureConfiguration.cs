using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class FeatureConfiguration : IEntityTypeConfiguration<FeatureEntity>
    {
        public void Configure(EntityTypeBuilder<FeatureEntity> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasMany(f => f.featureEquipments)
                .WithOne(fe => fe.Feature)
                .HasForeignKey(fe => fe.FeatureId);
        }
    }
}
