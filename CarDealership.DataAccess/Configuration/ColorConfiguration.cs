using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class ColorConfiguration : IEntityTypeConfiguration<ColorEntity>
    {
        public void Configure(EntityTypeBuilder<ColorEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Configurations)
                .WithOne(conf => conf.Color)
                .HasForeignKey(conf => conf.ColorId);
        }
    }
}
