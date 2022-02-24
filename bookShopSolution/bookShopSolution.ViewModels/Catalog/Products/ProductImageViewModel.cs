using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.Catalog.Products
{
    public class ProductImageViewModel
    {
        public int ImageId { get; set; }
        public string FilePath { get; set; }
        public string Caption { get; set; }
        public long FileSize { get; set; }
        public bool IsDefault { get; set; }
    }
}
