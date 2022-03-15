using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.Catalog.ProductRatings
{
    public class ProductRatingViewModel
    {
        public int Star { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
    }
}