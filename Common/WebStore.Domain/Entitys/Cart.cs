using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.Entitys
{
    public class Cart_Product
    {
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public int ItemsCount => Items?.Sum(item => item.Quantitie) ?? 0;
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantitie { get; set; } = 1;
    }
}
