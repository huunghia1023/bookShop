﻿namespace bookShopSolution.WebApp.Models
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
    }
}