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
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(x => new { x.ProductId, x.OrderId });
            builder.HasOne(o => o.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}