using bookShopSolution.Data.Entities;
using bookShopSolution.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Data.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotions");
            builder.HasKey(x => x.PromotionId);
            builder.Property(x => x.PromotionId).UseIdentityColumn();
            builder.Property(x => x.FromDay).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.Today).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.Status).HasDefaultValue(Status.Active);
            builder.Property(x => x.PromotionName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        }
    }
}
