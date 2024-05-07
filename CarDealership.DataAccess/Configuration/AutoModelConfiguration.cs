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

            builder.Property(am => am.Brand)
                .HasMaxLength(AutoModel.MAX_BRAND_LENGTH)
                .IsRequired();

            builder.Property(am => am.Name)
                .HasMaxLength(AutoModel.MAX_BRAND_LENGTH)
                .IsRequired();

            builder.Property(am => am.BodyType)
                .IsRequired();

            builder.Property(am => am.Price)
                .IsRequired();
        }
    }
}
