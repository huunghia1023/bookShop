using bookShopSolution.Data.EF;
using bookShopSolution.Data.Entities;
using bookShopSolution.Data.Enums;
using bookShopSolution.Utilities.Constants;
using bookShopSolution.ViewModels.Catalog.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Application.Catalog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly BookShopDbContext _context;

        public OrderService(BookShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(OrderCreateRequest request)
        {
            if (request.UserId == null || request.UserId == "")
            {
                request.UserId = (from u in _context.Users
                                  where u.UserName == SystemConstants.GuestAccount.Username && u.Email == SystemConstants.GuestAccount.Email
                                  select u.Id).FirstOrDefault().ToString();
            }
            if (request.ShipPhoneNumber == null)
            {
                request.ShipPhoneNumber = "";
            }
            if (request.Carts.Count == 0 || request.Carts == null)
            {
                return 0;
            }
            var orderDetails = new List<OrderDetail>();
            foreach (var item in request.Carts)
            {
                // get price
                var product = await _context.Products.FindAsync(item.ProductId);
                var price = product.Price * item.Quantity;
                var detail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = price
                };
                orderDetails.Add(detail);
            }
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                ShipEmail = request.ShipEmail,
                ShipAddress = request.ShipAddress,
                ShipName = request.ShipName,
                ShipPhoneNumber = request.ShipPhoneNumber,
                Status = OrderStatus.Confirmed,
                UserId = Guid.Parse(request.UserId),
                OrderDetails = orderDetails
            };
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<OrderViewModel> GetOrderById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return null;
            }
            var query = from od in _context.OrderDetails
                        join p in _context.Products on od.ProductId equals p.Id
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pi in _context.ProductImages on p.Id equals pi.ProductId
                        where od.OrderId == id && (pi == null || pi.IsDefault == true)
                        select new { od, p, pt, pi };
            var orderDetails = await query.Select(x => new OrderDetailViewModel()
            {
                Price = x.od.TotalPrice,
                ProductId = x.od.ProductId,
                Quantity = x.od.Quantity,
                Name = x.pt.ProductName,
                Thumbnail = x.pi.ImagePath
            }).ToListAsync();
            var orderResult = new OrderViewModel()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipName = order.ShipName,
                ShipPhoneNumber = order.ShipPhoneNumber,
                ShipStatus = order.Status,
                Details = orderDetails
            };
            return orderResult;
        }
    }
}