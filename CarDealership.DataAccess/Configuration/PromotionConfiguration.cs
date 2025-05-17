using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealership.DataAccess.Configuration
{
    public class PromotionConfiguration : IEntityTypeConfiguration<PromotionEntity>
    {
        public void Configure(EntityTypeBuilder<PromotionEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(p => p.Promocode)
                .IsUnique();

            builder.HasMany(p => p.AppliableConfigs)
                .WithMany();

            builder.HasMany(p => p.Participants)
                .WithMany();


            builder.ToTable(t => t.HasCheckConstraint("CK_Promotion_Discounts", "\"OrderDiscountPercent\" IS NOT NULL OR \"DealDiscountPercent\" IS NOT NULL"));
            builder.ToTable(t => t.HasCheckConstraint("CK_Promotion_Dates", "\"EndDate\" > \"StartDate\""));
        }
    }
}
