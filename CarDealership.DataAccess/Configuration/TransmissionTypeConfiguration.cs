using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class TransmissionTypeConfiguration : IEntityTypeConfiguration<TransmissionTypeEntity>
    {
        public void Configure(EntityTypeBuilder<TransmissionTypeEntity> builder)
        {
            builder.HasKey(tt => tt.Id);

            builder.HasMany(tt => tt.Engines)
                .WithOne(e => e.TransmissionType)
                .HasForeignKey(e => e.TransmissionTypeId);

            builder.HasIndex(tt => tt.Value)
                .IsUnique();
        }
    }
}
