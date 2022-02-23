﻿using bookShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(x => x.ImageId);
            builder.Property(x => x.ImageId).UseIdentityColumn();
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.Caption).HasMaxLength(200);
            builder.Property(x => x.IsDefault).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.DateCreated).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.SortOrder).IsRequired().HasDefaultValue(0);
        }
    }
}
