using bookShopSolution.Application.Catalog.Orders;
using bookShopSolution.ViewModels.Catalog.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bookShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return BadRequest("Order not found");
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var orderId = await _orderService.Create(request);
            if (orderId == 0) return BadRequest("Create failed");
            var orderCreated = await _orderService.GetOrderById(orderId);
            return CreatedAtAction(nameof(GetById), orderCreated);
        }
    }
}