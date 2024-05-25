using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class BodyTypeConfiguration : IEntityTypeConfiguration<BodyTypeEntity>
    {
        public void Configure(EntityTypeBuilder<BodyTypeEntity> builder)
        {
            builder.HasKey(bt => bt.Id);

            builder.HasMany(bt => bt.Configurations)
                .WithOne(c => c.BodyType)
                .HasForeignKey(c => c.BodyTypeId);
        }
    }
}
