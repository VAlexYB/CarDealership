using CarDealership.Core.Models.Auth;
using CarDealership.DataAccess.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Value)
               .IsRequired();

            builder.HasMany(r => r.Users)
                .WithMany(u => u.Roles);

            var roles = Enum
                .GetValues<Roles>()
                .Select(r => new RoleEntity
                {
                    Id = (int)r,
                    Value = r.ToString()
                });
            builder.HasData(roles);

        }
    }
}
