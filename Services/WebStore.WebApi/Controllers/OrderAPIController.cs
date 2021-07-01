using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Inerfaces;
using WebStore.Inerfaces.Services;
using static WebStore.Domain.DTO.OrderMapper;

namespace WebStore.WebApi.Controllers
{
    [Route(WebAPIAddress.Orders)]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly IOrderService _OrderService;

        public OrderAPIController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpGet("user/{UserName}")]
        public async Task<IActionResult> GetUserOrders(string UserName)
        {
            var orders = await _OrderService.GetUserOrders(UserName);
            return Ok(orders.ToDTO());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int Id)
        {
            var orders = await _OrderService.GetOrderById(Id);
            return Ok(orders.ToDTO());
        }

        [HttpPost("{UserName}")]
        public async Task<IActionResult> GreateOrder(string UserName, [FromBody] CreateOrderDTO OrderModel)
        {
            var orders = await _OrderService.CreateOrder(UserName, OrderModel.Items.ToCartView(), OrderModel.Order);
            return Ok(orders.ToDTO());
        }
    }
}
