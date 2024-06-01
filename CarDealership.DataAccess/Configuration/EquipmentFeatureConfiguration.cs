using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class EquipmentFeatureConfiguration : IEntityTypeConfiguration<EquipmentFeatureEntity>
    {
        public void Configure(EntityTypeBuilder<EquipmentFeatureEntity> builder)
        {
            builder.HasKey(ef => ef.Id);

            builder.HasOne(ef => ef.Equipment)
                .WithMany(e => e.equipmentFeatures)
                .HasForeignKey(ef => ef.EquipmentId);

            builder.HasOne(ef => ef.Feature)
                .WithMany(f => f.featureEquipments)
                .HasForeignKey(ef => ef.FeatureId);

            builder.HasIndex(ef => new {ef.EquipmentId, ef.FeatureId})
                .IsUnique();
        }
    }
}
