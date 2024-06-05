using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.AutoConfiguration)
                  .WithMany(ac => ac.Orders)
                  .HasForeignKey(o => o.AutoConfigurationId);

            builder.HasOne(o => o.Manager)
                   .WithMany(m => m.ManagedOrders)
                   .HasForeignKey(o => o.ManagerId);


            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.CustomerOrders)
                   .HasForeignKey(o => o.CustomerId);

            builder.Property(o => o.Status)
                   .HasConversion<int>();
        }
    }
}
