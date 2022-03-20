using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string? Details { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoTitle { get; set; }
        public string LanguageId { get; set; }
        public string? SeoAlias { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        //public IFormFile ThumbnailImage { get; set; }
    }
}