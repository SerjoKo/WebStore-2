using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entitys.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Inerfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrders(string UserName);

        Task<Order> GetOrderById(int id);

        Task<Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel);
    }
}
