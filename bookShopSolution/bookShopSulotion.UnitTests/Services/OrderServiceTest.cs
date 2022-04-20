using bookShopSolution.Application.Catalog.Categories;
using bookShopSolution.Application.Catalog.Orders;
using bookShopSolution.Data.EF;
using bookShopSolution.Data.Entities;
using bookShopSolution.Data.Enums;
using bookShopSolution.ViewModels.Catalog.Categories;
using bookShopSolution.ViewModels.Catalog.Orders;
using bookShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace bookShopSulotion.UnitTests.Services
{
    public class OrderServiceTest
    {
        private readonly BookShopDbContext _context;
        private readonly OrderService _orderService;

        public OrderServiceTest()
        {
            _context = GetInmemoryContext();

            _orderService = new OrderService(_context);
        }

        ~OrderServiceTest()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task Create_Return0_InvalidCart()
        {
            // Arrage
            var request = GetOrderCreateRequest(new List<CartViewModel>());

            var resultExpect = 0;

            // Act
            var actualResult = await _orderService.Create(request);

            // Assert
            Assert.Equal(resultExpect, actualResult);
        }

        [Fact]
        public async Task Create_Return0_ProductStockNotEnough()
        {
            // Arrage
            var cart = new List<CartViewModel>()
            {
                new CartViewModel()
                {
                    ProductId = 1,
                    Quantity = 3
                }
            };
            SeedProduct();
            var request = GetOrderCreateRequest(cart);

            var resultExpect = 0;

            // Act
            var actualResult = await _orderService.Create(request);

            // Assert
            Assert.Equal(resultExpect, actualResult);
        }

        [Fact]
        public async Task Create_ReturnOrderId_CreateSuccess()
        {
            // Arrage
            var cart = new List<CartViewModel>()
            {
                new CartViewModel()
                {
                    ProductId = 1,
                    Quantity = 1
                }
            };
            SeedProduct();
            var request = GetOrderCreateRequest(cart);

            var resultExpect = 1;

            // Act
            var actualResult = await _orderService.Create(request);

            // Assert
            Assert.Equal(resultExpect, actualResult);
        }

        [Fact]
        public async Task UpdateStatus_ReturnFalse_InvalidOrderId()
        {
            // Arrage

            SeedOrder();
            var invalidOrderId = 3;
            var resultExpect = false;

            // Act
            var actualResult = await _orderService.UpdateStatus(invalidOrderId, OrderStatus.Shipping);

            // Assert
            Assert.Equal(resultExpect, actualResult);
        }

        [Fact]
        public async Task UpdateStatus_ReturnTrue_UpdateStatusOrderSuccess()
        {
            // Arrage

            SeedOrder();
            var validOrderId = 1;
            var resultExpect = true;

            // Act
            var actualResult = await _orderService.UpdateStatus(validOrderId, OrderStatus.Shipping);

            // Assert
            Assert.Equal(resultExpect, actualResult);
        }

        private BookShopDbContext GetInmemoryContext()
        {
            var builder = new DbContextOptionsBuilder<BookShopDbContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            return new BookShopDbContext(builder.Options);
        }

        private OrderCreateRequest GetOrderCreateRequest(List<CartViewModel> cartViewModels)
        {
            return new OrderCreateRequest
            {
                ShipAddress = "Address test",
                ShipEmail = "emailtest@gmail.com",
                ShipName = "test name",
                ShipPhoneNumber = "0929929221",
                Carts = cartViewModels
            };
        }

        private async void SeedProduct()
        {
            _context.Products.Add(
                new Product()
                {
                    Id = 1,
                    Price = 10,
                    OriginalPrice = 11,
                    Stock = 1
                });
            await _context.SaveChangesAsync();
        }

        private async void SeedOrder()
        {
            _context.Orders.Add(
                new Order()
                {
                    Id = 1,
                    UserId = new Guid(),
                    Status = OrderStatus.Confirmed,
                    ShipAddress = "Address test",
                    ShipEmail = "emailtest@gmail.com",
                    ShipName = "Name Test",
                    ShipPhoneNumber = "091029012"
                });
            await _context.SaveChangesAsync();
        }
    }
}