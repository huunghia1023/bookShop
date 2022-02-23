﻿using bookShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookShopSolution.Data.Configurations
{
    public class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.ToTable("ProductInCategories");
            builder.HasKey(x => new { x.CategoryId, x.ProductId });
            builder.HasOne(t => t.Product).WithMany(pc => pc.ProductInCategories).HasForeignKey(pc=>pc.ProductId);
            builder.HasOne(t => t.Category).WithMany(pc => pc.ProductInCategories).HasForeignKey(pc=>pc.CategoryId);
        }


    }
}
