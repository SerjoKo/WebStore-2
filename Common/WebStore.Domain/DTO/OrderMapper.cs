using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entitys;
using WebStore.Domain.Entitys.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO
{
    public static partial class OrderMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem Item) => Item is null
            ? null
            : new OrderItemDTO
            {
                Id = Item.Id,
                ProductID = Item.Product.Id,
                Price = Item.Price,
                Quantity = Item.Quantity,
            };

        public static OrderItem FromDTO(this OrderItemDTO Item) => Item is null
            ? null
            : new OrderItem
            {
                Id = Item.Id,
                Product = new Product { Id = Item.Id },
                Price = Item.Price,
                Quantity = Item.Quantity,
            };

        public static OrderDTO ToDTO(this Order Order) => Order is null
            ? null
            : new OrderDTO
            {
                Id = Order.Id,
                Name = Order.Name,
                Address = Order.Adress,
                Phone = Order.Phone,
                Date = Order.Date,
                Items = Order.Items.Select(ToDTO)
            };

        public static Order FromDTO(this OrderDTO Order) => Order is null
            ? null
            : new Order
            {
                Id = Order.Id,
                Name = Order.Name,
                Adress = Order.Address,
                Phone = Order.Phone,
                Date = Order.Date,
                Items = Order.Items.Select(FromDTO).ToList()
            };

        public static IEnumerable<OrderDTO> ToDTO(this IEnumerable<Order> Orders) => Orders.Select(ToDTO);
        public static IEnumerable<Order> FromDTO(this IEnumerable<OrderDTO> Orders) => Orders.Select(FromDTO);

        public static IEnumerable<OrderItemDTO> ToDTO(this CartViewModel Cart) =>
            Cart.Items.Select(p => new OrderItemDTO
            {
                ProductID = p.Product.Id,
                Price = p.Product.Price,
                Quantity = p.Quantitie
            });

        public static CartViewModel ToCartView(this IEnumerable<OrderItemDTO> Items) =>
            new()
            {
                Items = Items.Select(p => (new ProductViewModel { Id = p.ProductID }, p.Quantity))
            };
    }
}
