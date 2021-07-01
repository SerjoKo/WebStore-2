using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.ViewModels;
using WebStore.Inerfaces;
using WebStore.Inerfaces.Services;
using WebStore.WebApi.lients.Base;
using static WebStore.Domain.DTO.OrderMapper;

namespace WebStore.WebApi.Clients.Order
{
    public class OrderClient : BaseClient, IOrderService
    {
        public OrderClient(HttpClient Client) : base(Client, WebAPIAddress.Orders)
        { }

        public async Task<Domain.Entitys.Orders.Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel)
        {
            var create_order_model = new CreateOrderDTO
            {
                Items = Cart.ToDTO(),
                Order = OrderModel,
            };
            var response = await PostAsync($"{Address}/{UserName}", create_order_model).ConfigureAwait(false);
            var order_dto = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<OrderDTO>().ConfigureAwait(false);
            return order_dto.FromDTO();
        }

        public async Task<Domain.Entitys.Orders.Order> GetOrderById(int id)
        {
            var order_dto = await GetAsync<OrderDTO>($"{Address}/{id}").ConfigureAwait(false);
            return order_dto.FromDTO();
        }

        public async Task<IEnumerable<Domain.Entitys.Orders.Order>> GetUserOrders(string UserName)
        {
            var orders_dto = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{UserName}").ConfigureAwait(false);
            return orders_dto.FromDTO();
        }
    }
}
