using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<CountryEntity>
    {
        public void Configure(EntityTypeBuilder<CountryEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Brands)
                .WithOne(b => b.Country)
                .HasForeignKey(b => b.CountryId);

            builder.HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
