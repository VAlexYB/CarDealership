using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class CarConfiguration : IEntityTypeConfiguration<CarEntity>
    {
        public void Configure(EntityTypeBuilder<CarEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.AutoConfiguration)
                .WithMany(ac => ac.Cars)
                .HasForeignKey(c => c.AutoConfigurationId);

            builder.HasIndex(c => c.VIN)
                .IsUnique();
        }
    }
}
