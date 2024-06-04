using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class AutoConfConfiguration : IEntityTypeConfiguration<AutoConfigurationEntity>
    {
        public void Configure(EntityTypeBuilder<AutoConfigurationEntity> builder)
        {
            builder.HasKey(ac => ac.Id);

            builder.Property(ac => ac.Price)
                .IsRequired();

            builder.HasOne(ac => ac.AutoModel)
                .WithMany(am => am.Configurations)
                .HasForeignKey(ac => ac.AutoModelId);

            builder.HasOne(ac => ac.BodyType)
                .WithMany(bt => bt.Configurations)
                .HasForeignKey(ac => ac.BodyTypeId);

            builder.HasOne(ac => ac.DriveType)
                .WithMany(dt => dt.Configurations)
                .HasForeignKey(ac => ac.DriveTypeId);

            builder.HasOne(ac => ac.Engine)
                .WithMany(e => e.Configurations)
                .HasForeignKey(ac => ac.EngineId);

            builder.HasOne(ac => ac.Color)
                .WithMany(c => c.Configurations)
                .HasForeignKey(ac => ac.ColorId);

            builder.HasOne(ac => ac.Equipment)
                .WithMany(e => e.Configurations)
                .HasForeignKey(ac => ac.EquipmentId);

            builder.HasMany(ac => ac.Cars)
                .WithOne(c => c.AutoConfiguration)
                .HasForeignKey(c => c.AutoConfigurationId);

        }
    }
}
