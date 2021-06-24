using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using WebStore.Domain;
using WebStore.Domain.Entitys;
using WebStore.Inftastructure.Mapping;
using WebStore.Servicess.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Servicess.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _HttpContextAccesor;
        private readonly IProductData _ProductData;
        private readonly string _CartName;

        private Cart_Product Cart
        {
            get
            {
                var context = _HttpContextAccesor.HttpContext;

                var cookies = context.Response.Cookies;

                var cart_cookie = context.Request.Cookies[_CartName];
                if (cart_cookie is null)
                {
                    var cart = new Cart_Product();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookies(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart_Product>(cart_cookie);
            }
            set => ReplaceCookies(
                    _HttpContextAccesor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCookies(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie);
        }

        public InCookiesCartService(IHttpContextAccessor httpContextAccesor, IProductData ProductData)
        {
            _HttpContextAccesor = httpContextAccesor;
            _ProductData = ProductData;

            var user = _HttpContextAccesor.HttpContext!.User;
            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WS.Cart{user_name}";
        }

        public void Add(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == Id);

            if (item is null)
                cart.Items.Add(new CartItem { ProductId = Id });
            else
                item.Quantitie++;

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }
       
        public void Decrement(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == Id);

            if (item is null) return;
            
            if (item.Quantitie > 0)
                item.Quantitie--;

            if (item.Quantitie <= 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public CartViewModel GetCartViewModel()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var products_views = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items
                   .Where(item => products_views.ContainsKey(item.ProductId))
                   .Select(item => (products_views[item.ProductId], item.Quantitie))
            };
        }  

        public void Remove(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null) return;

            cart.Items.Remove(item);

            Cart = cart;

        }
    }
}
