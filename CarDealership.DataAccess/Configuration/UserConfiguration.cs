using CarDealership.DataAccess.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users);

            builder.HasIndex(u => u.UserName)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasMany(u => u.ManagedOrders)
                   .WithOne(o => o.Manager)
                   .HasForeignKey(o => o.ManagerId);

            builder.HasMany(u => u.CustomerOrders)
                   .WithOne(o => o.Customer)
                   .HasForeignKey(o => o.CustomerId);

            builder.HasMany(u => u.ManagedDeals)
                   .WithOne(d => d.Manager)
                   .HasForeignKey(d => d.ManagerId);

            builder.HasMany(u => u.CustomerDeals)
                   .WithOne(d => d.Customer)
                   .HasForeignKey(d => d.CustomerId);
        }
    }
}
