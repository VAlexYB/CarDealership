using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<EquipmentEntity>
    {
        public void Configure(EntityTypeBuilder<EquipmentEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.AutoModel)
                .WithMany(am => am.Equipments)
                .HasForeignKey(e => e.AutoModelId);

            builder.HasMany(e => e.equipmentFeatures)
                .WithOne(ef => ef.Equipment)
                .HasForeignKey(ef => ef.EquipmentId);

            builder.HasMany(e => e.Configurations)
                .WithOne(ac => ac.Equipment)
                .HasForeignKey(ac => ac.EquipmentId);
        }
    }
}
