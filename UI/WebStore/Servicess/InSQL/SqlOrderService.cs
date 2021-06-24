using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context.WebStore.DAL.Context;
using WebStore.Domain.Entitys.Identity;
using WebStore.Domain.Entitys.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Inerfaces.Services;

namespace WebStore.Servicess.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public async Task<Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel)
        {
            var user = await _UserManager.FindByNameAsync(UserName);
            if (user is null)
                throw new InvalidOperationException($"Пользователь {UserName} нет в БД");

            await using var transaction = await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                User = user,
                Adress = OrderModel.Adress,
                Phone = OrderModel.Phone,
                Name = OrderModel.Name,
            };

            var product_id = Cart.Items.Select(item => item.Product.Id).ToArray();

            var cart_products = await _db.Products
               .Where(p => product_id.Contains(p.Id))
               .ToArrayAsync();

            order.Items = Cart.Items.Join(
                cart_products,
                cart_item => cart_item.Product.Id,
                cart_product => cart_product.Id,
                (cart_item, cart_product) => new OrderItem
                {
                    Order = order,
                    Product = cart_product,
                    Price = cart_product.Price, // здесь можно применить скидки...
                    Quantity = cart_item.Quantitie,
                }).ToArray();

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            await transaction.CommitAsync();

            return order;
        }

        public async Task<Order> GetOrderById(int id) =>
            await _db.Orders
           .Include(order => order.User)
           .Include(order => order.Items)
           .FirstOrDefaultAsync(order => order.Id == id);

        public async Task<IEnumerable<Order>> GetUserOrder(string UserName) => 
            await _db.Orders
           .Include(order => order.User)
           .Include(order => order.Items)
           .Where(order => order.User.UserName == UserName)
           .ToArrayAsync();
    }
}
