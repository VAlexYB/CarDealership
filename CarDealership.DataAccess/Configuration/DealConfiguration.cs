using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class DealConfiguration : IEntityTypeConfiguration<DealEntity>
    {
        public void Configure(EntityTypeBuilder<DealEntity> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Car)
                  .WithMany()
                  .HasForeignKey(o => o.CarId);

            builder.HasOne(d => d.Manager)
                   .WithMany(m => m.ManagedDeals)
                   .HasForeignKey(d => d.ManagerId);


            builder.HasOne(d => d.Customer)
                   .WithMany(c => c.CustomerDeals)
                   .HasForeignKey(d => d.CustomerId);

            builder.Property(o => o.Status)
                   .HasConversion<int>();
        }
    }
}
