using System.Collections.Generic;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO
{
    public static partial class OrderMapper
    {
        public class CreateOrderDTO
        {
            /// <summary>Модель заказа</summary>
            public OrderViewModel Order { get; set; }

            /// <summary>Пункты заказа</summary>
            public IEnumerable<OrderItemDTO> Items { get; set; }
        }
    }
}
