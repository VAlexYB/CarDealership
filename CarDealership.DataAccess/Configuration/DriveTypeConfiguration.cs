using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class DriveTypeConfiguration : IEntityTypeConfiguration<DriveTypeEntity>
    {
        public void Configure(EntityTypeBuilder<DriveTypeEntity> builder)
        {
            builder.HasKey(dt => dt.Id);

            builder.HasMany(dt => dt.Configurations)
                .WithOne(c => c.DriveType)
                .HasForeignKey(c => c.DriveTypeId);
        }
    }
}
