using System;
using System.Collections.Generic;
using WebStore.Domain.Entitys;
using WebStore.Domain.Entitys.Orders;

namespace WebStore.Domain.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }

    
}
