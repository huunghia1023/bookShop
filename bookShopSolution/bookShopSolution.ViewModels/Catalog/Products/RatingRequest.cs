using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.Catalog.Products
{
    public class RatingRequest
    {
        public Guid UserId { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
    }
}