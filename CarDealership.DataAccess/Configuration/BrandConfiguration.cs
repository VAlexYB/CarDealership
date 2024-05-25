using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class BrandConfiguration : IEntityTypeConfiguration<BrandEntity>
    {
        public void Configure(EntityTypeBuilder<BrandEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasMany(b => b.Models)
                .WithOne(am => am.Brand)
                .HasForeignKey(am => am.BrandId);

            builder.HasOne(b => b.Country)
                .WithMany(c => c.Brands)
                .HasForeignKey(b => b.CountryId);
        }
    }
}
