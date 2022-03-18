using bookShopSolution.ViewModels.Catalog.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Application.Catalog.Orders
{
    public interface IOrderService
    {
        public Task<int> Create(OrderCreateRequest request);

        public Task<OrderViewModel> GetOrderById(int id);
    }
}