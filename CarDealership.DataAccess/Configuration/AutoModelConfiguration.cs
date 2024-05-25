using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class AutoModelConfiguration : IEntityTypeConfiguration<AutoModelEntity>
    {
        public void Configure(EntityTypeBuilder<AutoModelEntity> builder)
        {
            builder.HasKey(am => am.Id);

            builder.Property(am => am.Name)
                .HasMaxLength(AutoModel.MAX_NAME_LENGTH)
                .IsRequired();

            builder.HasOne(am => am.Brand)
                .WithMany(b => b.Models);

            builder.HasMany(am => am.Configurations)
                .WithOne(c => c.AutoModel)
                .HasForeignKey(c => c.AutoModelId);

            builder.HasMany(am => am.Equipments)
                .WithOne(e => e.AutoModel)
                .HasForeignKey(e => e.AutoModelId);

        }
    }
}
