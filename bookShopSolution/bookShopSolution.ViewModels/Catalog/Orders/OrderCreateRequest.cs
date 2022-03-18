using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.Catalog.Orders
{
    public class OrderCreateRequest
    {
        public string ShipEmail { get; set; }
        public string ShipAddress { get; set; }
        public string ShipName { get; set; }
        public string? ShipPhoneNumber { get; set; }
        public string? UserId { get; set; }
        public List<CartViewModel> Carts { get; set; }
    }
}