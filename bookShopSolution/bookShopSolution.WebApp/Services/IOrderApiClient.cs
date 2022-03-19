using bookShopSolution.ViewModels.Catalog.Orders;

namespace bookShopSolution.WebApp.Services
{
    public interface IOrderApiClient
    {
        public Task<OrderViewModel> Create(OrderCreateRequest request);

        public Task<OrderViewModel> GetById(int id);

        public Task<bool> CancelOrder(int id);
    }
}